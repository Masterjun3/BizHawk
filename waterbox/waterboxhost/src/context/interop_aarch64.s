.section .text

// Constant definitions (replaces %define)
.equ BASE_ADDR, 0x35f00000000
.macro RVA addr
    (\addr - BASE_ADDR)
.endm

// Structures in GAS are typically handled via offset definitions
// GAS does not have a native 'struc' block like NASM
.equ Context_thread_area,      0
.equ Context_host_rsp,        8
.equ Context_guest_rsp,       16
.equ Context_host_rsp_alt,    24
.equ Context_guest_rsp_alt,   32
.equ Context_dispatch_syscall, 40
.equ Context_host_ptr,        48
.equ Context_extcall_slots,   56
.equ Context_SIZE,            56 + (64 * 8)

start:

.space 0x80 - (. - start)

.global guest_syscall
.type guest_syscall, %function

guest_syscall:

    stp x30, xzr, [sp, #-16]!      // push x30 for ret

    mrs     x9, tpidr_el0          // x9 = Context*

    mov     x10, sp
    str     x10, [x9, #16]         // Context.guest_rsp

    ldr     x10, [x9, #8]          // Context.host_rsp
    mov     sp, x10

    mov     x6, x8                 // syscall number
    ldr     x7, [x9, #48]          // Context.host_ptr

    ldr     x10, [x9, #40]         // Context.dispatch_syscall
    blr     x10

    mrs     x9, tpidr_el0          // x9 = Context*

    ldr     x10, [x9, #16]         // Context.guest_rsp
    mov     sp, x10

    ldp x30, xzr, [sp], #16        // restore x30
    ret

.space 0x100 - (. - start)

.global call_guest_simple
.type call_guest_simple, %function

call_guest_simple:
    mov     x11, x0        // guest entry
    mov     x10, x1        // Context*
    b       call_guest_impl

.space 0x200 - (. - start)

.global call_guest_impl
.type call_guest_impl, %function

call_guest_impl:
    stp x30, xzr, [sp, #-16]!      // push x30 for ret

    ldr     x9, [x10, #8]          // host_rsp
    cbz     x9, do_switch          // if zero → normal case

    ldr     x9, [x10, #24]         // host_rsp_alt
    cbz     x9, do_swap

    brk     #0                     // both stacks exhausted

do_swap:
    // host_rsp_alt = host_rsp
    ldr     x9, [x10, #8]
    str     x9, [x10, #24]

    // swap guest_rsp <-> guest_rsp_alt
    ldr     x9, [x10, #16]
    ldr     x12, [x10, #32]
    str     x12, [x10, #16]
    str     x9,  [x10, #32]

do_switch:
    mrs     x9, tpidr_el0
    str     x9, [sp, #-16]!

    mov     x9, sp
    str     x9, [x10, #8]          // host_rsp = sp

    ldr     x9, [x10, #16]         // guest_rsp
    mov     sp, x9

    msr     tpidr_el0, x10

    blr     x11                    // return address goes on guest stack

    // Reload Context*
    mrs     x10, tpidr_el0

    // Save updated guest SP
    mov     x9, sp
    str     x9, [x10, #16]

    // Restore host SP
    ldr     x9, [x10, #8]
    mov     sp, x9

    // Mark this host stack inactive
    mov     x9, xzr
    str     x9, [x10, #8]

    ldr     x9, [x10, #24]         // host_rsp_alt
    cbz     x9, done

    // host_rsp = host_rsp_alt
    str     x9, [x10, #8]
    str     xzr, [x10, #24]

    // swap guest_rsp <-> guest_rsp_alt back
    ldr     x9,  [x10, #16]
    ldr     x12, [x10, #32]
    str     x12, [x10, #16]
    str     x9,  [x10, #32]

done:
    ldr x9, [sp], #16
    msr tpidr_el0, x9

    ldp x30, xzr, [sp], #16        // restore x30
    ret

.space 0x300 - (. - start)

.macro guest_extcall_thunk slot
    mov     x8, #\slot
    b       guest_extcall_impl
    .align  4
.endm

.global guest_extcall_thunks
guest_extcall_thunks:

.set i,0
.rept 64
    guest_extcall_thunk i
    .set i, i+1
.endr

.global guest_extcall_impl
.type guest_extcall_impl, %function

guest_extcall_impl:
    stp x30, xzr, [sp, #-16]!      // push x30 for ret

    mrs     x10, tpidr_el0         // Context*

    // Save guest SP
    mov     x9, sp
    str     x9, [x10, #16]

    // Switch to host stack
    ldr     x9, [x10, #8]
    mov     sp, x9

    // Lookup slot function
    add     x11, x10, #56          // &extcall_slots[0]
    ldr     x11, [x11, x8, lsl #3]

    blr     x11

    // Restore guest stack
    ldr     x9, [x10, #16]
    mov     sp, x9

    ldp x30, xzr, [sp], #16        // restore x30
    ret

.space 0x800 - (. - start)
