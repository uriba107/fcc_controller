# Portable AVR-GCC makefile for the FCC3 controller firmware (ATmega32U4).
#
# Builds with any avr-gcc / avr-libc toolchain (Atmel Studio's, Debian/Ubuntu's
# gcc-avr, MSYS2, Homebrew, ...), so Atmel Studio is no longer required.
#
# Usage:
#   make                      Release build into ./build
#   make DEBUG=1              Debug build (-Og -g3)
#   make clean                Remove ./build
#   make package              Also emit fcc_controller_<version>.hex
#   make size                 Show flash/SRAM usage
#
# Useful variables (all optional):
#   AVR_PREFIX=/opt/avr/bin/avr-   Toolchain prefix (default: avr-)
#   BUILD_DIR=out                  Output directory (default: build)
#   MCU=atmega32u4                 Target device

TARGET     ?= Fcc3
MCU        ?= atmega32u4
F_CPU      ?= 16000000UL
F_USB      ?= 16000000UL
BUILD_DIR  ?= build
AVR_PREFIX ?= avr-

CC      := $(AVR_PREFIX)gcc
OBJCOPY := $(AVR_PREFIX)objcopy
OBJDUMP := $(AVR_PREFIX)objdump
SIZE    := $(AVR_PREFIX)size

ELF  := $(BUILD_DIR)/$(TARGET).elf
HEX  := $(BUILD_DIR)/$(TARGET).hex
EEP  := $(BUILD_DIR)/$(TARGET).eep
LSS  := $(BUILD_DIR)/$(TARGET).lss
SREC := $(BUILD_DIR)/$(TARGET).srec

# Firmware version reported over USB (Descriptors.c -> ReleaseNumber).
VERSION := $(shell sed -n 's/.*ReleaseNumber *=[^V]*VERSION_BCD *(\ *\([0-9]*\) *, *\([0-9]*\) *, *\([0-9]*\) *).*/\1.\2.\3/p' Descriptors.c 2>/dev/null | head -n 1)

# --------------------------------------------------------------------------
# Sources. Application sources first, then the LUFA modules the USB stack
# needs (same file set the Atmel Studio project compiles).
# --------------------------------------------------------------------------
SRCS := \
	Descriptors.c \
	fcc3.c \
	fcc3_helpers.c \
	Joystick.c \
	src/LUFA/LUFA/Drivers/USB/Class/Common/HIDParser.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/AudioClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/CDCClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/HIDClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/MassStorageClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/MIDIClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/PrinterClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Device/RNDISClassDevice.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/AndroidAccessoryClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/AudioClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/CDCClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/HIDClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/MassStorageClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/MIDIClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/PrinterClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/RNDISClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Class/Host/StillImageClassHost.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Device_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/EndpointStream_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Endpoint_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Host_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/PipeStream_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Pipe_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Template/Template_Endpoint_Control_R.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Template/Template_Endpoint_Control_W.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Template/Template_Endpoint_RW.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/Template/Template_Pipe_RW.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/USBController_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/AVR8/USBInterrupt_AVR8.c \
	src/LUFA/LUFA/Drivers/USB/Core/ConfigDescriptors.c \
	src/LUFA/LUFA/Drivers/USB/Core/DeviceStandardReq.c \
	src/LUFA/LUFA/Drivers/USB/Core/Events.c \
	src/LUFA/LUFA/Drivers/USB/Core/HostStandardReq.c \
	src/LUFA/LUFA/Drivers/USB/Core/USBTask.c

OBJS := $(addprefix $(BUILD_DIR)/,$(SRCS:.c=.o))
DEPS := $(OBJS:.o=.d)

# --------------------------------------------------------------------------
# Flags (kept in sync with Fcc3.cproj)
# --------------------------------------------------------------------------
CPPFLAGS += -DARCH=ARCH_AVR8 \
            -DF_CPU=$(F_CPU) \
            -DF_USB=$(F_USB) \
            -DBOARD=BOARD_NONE \
            -DUSE_LUFA_CONFIG_HEADER \
            -I. -Isrc -Isrc/config -Isrc/LUFA

CFLAGS += -mmcu=$(MCU) \
          -std=gnu99 \
          -funsigned-char \
          -funsigned-bitfields \
          -fpack-struct \
          -fshort-enums \
          -ffunction-sections \
          -fdata-sections \
          -fno-strict-aliasing \
          -fno-jump-tables \
          -mrelax \
          -Wall \
          -MMD -MP

ifeq ($(DEBUG),1)
CPPFLAGS += -DDEBUG
CFLAGS   += -Og -g3
else
CPPFLAGS += -DNDEBUG
CFLAGS   += -Os
endif

LDFLAGS += -mmcu=$(MCU) -mrelax -Wl,--relax -Wl,--gc-sections -Wl,-Map=$(BUILD_DIR)/$(TARGET).map
LDLIBS  += -lm

# --------------------------------------------------------------------------
.PHONY: all hex eep lss srec package size clean help

all: $(HEX) $(EEP) $(LSS) $(SREC)
	@$(SIZE) --mcu=$(MCU) --format=avr $(ELF)

hex: $(HEX)
eep: $(EEP)
lss: $(LSS)
srec: $(SREC)

$(BUILD_DIR)/%.o: %.c
	@mkdir -p $(dir $@)
	$(CC) $(CPPFLAGS) $(CFLAGS) -c $< -o $@

$(ELF): $(OBJS)
	$(CC) $(LDFLAGS) $(OBJS) $(LDLIBS) -o $@

$(HEX): $(ELF)
	$(OBJCOPY) -O ihex -R .eeprom $< $@

$(EEP): $(ELF)
	$(OBJCOPY) -j .eeprom --set-section-flags=.eeprom=alloc,load \
		--change-section-lma .eeprom=0 --no-change-warnings -O ihex $< $@

$(LSS): $(ELF)
	$(OBJDUMP) -h -S $< > $@

$(SREC): $(ELF)
	$(OBJCOPY) -O srec $< $@

package: all
	@if [ -z "$(VERSION)" ]; then \
		echo "Could not read firmware version from Descriptors.c" >&2; exit 1; \
	fi
	cp $(HEX) "fcc_controller_$(VERSION).hex"
	cp $(EEP) "fcc_controller_$(VERSION).eep"
	@echo "Release assets: fcc_controller_$(VERSION).hex, fcc_controller_$(VERSION).eep"

size: $(ELF)
	@$(SIZE) --mcu=$(MCU) --format=avr $(ELF)

clean:
	rm -rf $(BUILD_DIR)

help:
	@echo "Targets: all (default), hex, eep, lss, srec, package, size, clean"
	@echo "Options: DEBUG=1, BUILD_DIR=<dir>, MCU=<device>, AVR_PREFIX=<prefix>"

-include $(DEPS)
