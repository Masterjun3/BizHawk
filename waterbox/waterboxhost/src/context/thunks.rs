use crate::*;
use memory_block::{Protection, pal};
use context::{CALL_GUEST_IMPL_ADDR, Context};
use std::collections::HashMap;

// each generated thunk should look like this:
// 49 ba .. .. .. .. .. .. .. ..             mov r10, <host_context_ptr>
// 49 bb .. .. .. .. .. .. .. ..             mov r11, <guest_entry_point>
// 48 b8 .. .. .. .. .. .. .. ..             mov rax, <call_guest_impl>
// ff e0                                     jmp rax

#[cfg(target_arch = "x86_64")]
const THUNK_SIZE: usize = 32;

#[cfg(target_arch = "aarch64")]
const THUNK_SIZE: usize = 64;
//  5800008a     ldr x10, 10
//  580000ab     ldr x11, 18
//  580000d0     ldr x16, 20
//  d61f0200     br  x16
//  .dword  <host_context_ptr>
//  .dword  <guest_entry_point>
//  .dword  <call_guest_impl>

/// tracks thunks for calling into waterbox code
pub struct ThunkManager {
	memory: AddressRange,
	lookup: HashMap<usize, usize>,
}
impl ThunkManager {
	pub fn new() -> anyhow::Result<ThunkManager> {
		let addr = pal::map_anon(AddressRange { start: 0, size: PAGESIZE }, Protection::RWX)?;
		Ok(ThunkManager {
			memory: addr,
			lookup: HashMap::new(),
		})
	}
	/// Generates a thunk for calling into waterbox.
	/// Only valid so long as this ThunkManager is alive and set_context_ptr is kept up to date
	/// See also `call_guest_simple`, which directly performs a call of the sort that these thunks do, but requires
	/// Context to be passed at the time of call and cannot pass arguments to the guest
	#[cfg(target_arch = "x86_64")]
	pub fn get_thunk_for_proc(&mut self, guest_entry_point: usize, context: *mut Context) -> anyhow::Result<usize> {
		match self.lookup.get(&guest_entry_point) {
			Some(p) => return Ok(*p),
			None => ()
		}
		let offset = self.lookup.len() * THUNK_SIZE;
		let p = self.memory.start + offset;
		if p >= self.memory.end() {
			Err(anyhow!("No room for another thunk!"))
		} else {
			unsafe {
				let dest = &mut &mut self.memory.slice_mut()[offset..offset + THUNK_SIZE];
				// mov r10, <host_context_ptr>
				bin::writeval::<u8>(dest, 0x49)?;
				bin::writeval::<u8>(dest, 0xba)?;
				bin::writeval(dest, context as usize)?;

				// mov r11, <guest_entry_point>
				bin::writeval::<u8>(dest, 0x49)?;
				bin::writeval::<u8>(dest, 0xbb)?;
				bin::writeval(dest, guest_entry_point as usize)?;

				// mov rax, <call_guest_impl>
				bin::writeval::<u8>(dest, 0x48)?;
				bin::writeval::<u8>(dest, 0xb8)?;
				bin::writeval(dest, CALL_GUEST_IMPL_ADDR)?;

				// jmp rax
				bin::writeval::<u8>(dest, 0xff)?;
				bin::writeval::<u8>(dest, 0xe0)?;

				self.lookup.insert(guest_entry_point, p);
				Ok(p)
			}
		}
	}
	// /// updates context value for all previously created thunks
	// pub fn update_context_ptr(&mut self, context: *mut Context) -> anyhow::Result<()> {
	// 	unsafe {
	// 		let slice = self.memory.slice_mut();
	// 		for i in 0..self.lookup.len() {
	// 			bin::writeval(&mut &mut slice[i * THUNK_SIZE + 2..i * THUNK_SIZE + 10], context as usize)?;
	// 		}
	// 	}
	// 	Ok(())
	// }

	#[cfg(target_arch = "aarch64")]
    pub fn get_thunk_for_proc(&mut self, guest_entry_point: usize, context: *mut Context) -> anyhow::Result<usize> {
        match self.lookup.get(&guest_entry_point) {
            Some(p) => return Ok(*p),
            None => ()
        }
        let offset = self.lookup.len() * THUNK_SIZE;
        let p = self.memory.start + offset;
        if p >= self.memory.end() {
            Err(anyhow!("No room for another thunk!"))
        } else {
            unsafe {
                let dest = &mut &mut self.memory.slice_mut()[offset..offset + THUNK_SIZE];
                // ldr x10, 10
                bin::writeval::<u32>(dest, 0x5800008a)?;

                // ldr x11, 18
                bin::writeval::<u32>(dest, 0x580000ab)?;

                // ldr x16, 20
                bin::writeval::<u32>(dest, 0x580000d0)?;

                // br x16
                bin::writeval::<u32>(dest, 0xd61f0200)?;

                // .dword <host_context_ptr>
                bin::writeval(dest, context as usize)?;

                // .dword <guest_entry_point>
                bin::writeval(dest, guest_entry_point as usize)?;

                // .dword <call_guest_impl>
                bin::writeval(dest, CALL_GUEST_IMPL_ADDR)?;

                self.lookup.insert(guest_entry_point, p);
                Ok(p)
            }
        }
    }

}
impl Drop for ThunkManager {
	fn drop(&mut self) {
		unsafe { pal::unmap_anon(self.memory).unwrap(); }
	}
}
