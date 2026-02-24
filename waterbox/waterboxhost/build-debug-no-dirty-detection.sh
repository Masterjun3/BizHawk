#!/bin/sh
if [ -z "$BIZHAWKBUILD_HOME" ]; then export BIZHAWKBUILD_HOME="$(realpath "$(dirname "$0")/../..")"; fi

cargo b --features "no-dirty-detection" --target aarch64-unknown-linux-gnu

cp target/aarch64-unknown-linux-gnu/debug/libwaterboxhost.so "$BIZHAWKBUILD_HOME/Assets/dll"
if [ -e "$BIZHAWKBUILD_HOME/output" ]; then
	cp target/aarch64-unknown-linux-gnu/debug/libwaterboxhost.so "$BIZHAWKBUILD_HOME/output/dll"
fi
