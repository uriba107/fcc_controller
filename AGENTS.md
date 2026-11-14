# FCC3 Controller — Agent Notes

USB adapter firmware + Windows configurator for the FCC3 force-sensing Cougar/Warthog mod.
Two independent stacks, no shared build, no test suite, no lint/format config. Compilation is the only local verification.

## Layout

- `src/Controller/Fcc3/` — AVR C firmware for ATmega32U4 (16 MHz) using vendored LUFA 170418.
  - `Joystick.c` — USB setup + main loop; `fcc3.c` — axis/force math, grip read, config mode, EEPROM; `Descriptors.c` — USB/HID descriptors and versions.
  - `src/LUFA/**` and `src/config/LUFAConfig.h` — vendored/library code, don't hand-edit.
- `src/App/` — .NET 10 (`net10.0-windows`) WinForms configurator plus helper libraries and a WiX installer.
  - `Fcc3_configurator.sln` → `FCC_configurator` (main app), `GithubUpdater`, `HexHelper`.
  - `Installer/` → WiX v5 `Installer.wixproj` (MSI) and `Bundle.wixproj` (Burn `Setup.exe`); built with `dotnet build`, no Visual Studio required.
  - `FccInstaller/` is a legacy `.vdproj` installer not in the solution.
- `docs/` (manuals), `Schematics/` (Eagle), `misc/` (cheat sheet, drivers). README is user/behavior documentation, not build docs.

## Building

### Firmware
- Primary build: `make -C src/Controller/Fcc3` (portable AVR-GCC Makefile; works on Linux/macOS/MSYS2 and with Atmel Studio's own toolchain via `AVR_PREFIX`). Output is `src/Controller/Fcc3/build/Fcc3.hex`.
- `make DEBUG=1` = `-Og -g3 -DDEBUG`; `make package` also writes `fcc_controller_<version>.hex/.eep`; `make size`, `make clean`. Version comes from `ReleaseNumber = VERSION_BCD(...)` in `Descriptors.c`.
- Needs only `avr-gcc` + `avr-libc` (Ubuntu/Debian: `gcc-avr avr-libc binutils-avr`). No Atmel DFP include path: `avr-libc`'s `avr/io.h` provides the ATmega32U4 headers. Device is ATmega32U4 @ 16 MHz.
- `src/Controller/Fcc3/Makefile` is the source of truth for the source list and flags. When adding/removing a `.c` file or changing flags, update `Makefile` and `Fcc3.cproj` together (or drop the Studio project).
- Atmel Studio (optional, still supported): open `src/Controller/Fcc3.atsln`, build Release → `src/Controller/Fcc3/Release/Fcc3.hex`. `Release/`/`Debug/` and their auto-generated Makefiles hardcode `C:\Program Files (x86)\Atmel\Studio\7.0\...` — don't edit them; they are gitignored stale artifacts.
- Build flags that matter: `-DARCH=ARCH_AVR8 -DF_CPU=16000000UL -DF_USB=16000000UL -DBOARD=BOARD_NONE -DUSE_LUFA_CONFIG_HEADER`, `-std=gnu99`, `-Os`, `-fpack-struct -fshort-enums`, `-ffunction-sections -fdata-sections -Wl,--gc-sections`, `-mrelax`.
- CI: `.github/workflows/firmware.yml` builds on Ubuntu (apt `gcc-avr avr-libc binutils-avr`) and uploads a `firmware-<version>` artifact on every push/PR. `.github/workflows/app.yml` builds the WinForms app on `windows-latest` with `actions/setup-dotnet@v6` (10.0.x), then publishes the app and builds the WiX MSI + Burn bundle. Versions are read from code, never typed: firmware from `VERSION_BCD(...)` in `Descriptors.c`, app from `AssemblyInfo.cs`.
- Releases: no manual tags. Run either workflow with **Run workflow → Publish release** (workflow_dispatch with `release=true`); CI names/creates the tag itself (`fw-v<version>` / `app-v<version>` at the dispatched commit) and carries the *other* component's assets forward from the newest previous release that has them, so every release still contains `.hex` plus the app installer. Push/PR triggers are path-filtered (`src/Controller/**` vs `src/App/**`) and only build. `GithubUpdater` is null-safe on a missing component anyway (shows 0.0.0 / disables the update button).

### Configurator app
- The app and its helper libraries are SDK-style projects targeting .NET 10. Build with `dotnet build` (requires the .NET 10 SDK). Build the app project to also build `GithubUpdater` and `HexHelper`:
  ```powershell
  dotnet build src/App/Fcc3_configurator/FCC_configurator.csproj -c Release
  ```
- Output is `src/App/Fcc3_configurator/bin/Release/net10.0-windows/`. The app is framework-dependent: running it needs the .NET 10 Windows Desktop Runtime.
- PackageReferences live in the `.csproj` files (no `packages.config`): `HidSharp` 2.6.4 and `WinForms.DataVisualization` 1.10.2 in the app, `System.IO.Ports` 10.0.14 in `HexHelper`. `System.Configuration.ConfigurationManager` is in the .NET 10 Windows Desktop shared framework, so it is not referenced explicitly.
- Installer: `src/App/Installer/` is a WiX Toolset v5 SDK project (`WixToolset.Sdk` 5.0.2, restored via NuGet). No Visual Studio or Installer Projects extension is needed:
  ```powershell
  dotnet publish src/App/Fcc3_configurator/FCC_configurator.csproj -c Release
  dotnet build src/App/Installer/Bundle.wixproj -c Release
  ```
  `Installer.wixproj` produces the framework-dependent MSI (`bin/Release/<culture>/Fcc3Controller.msi`, harvesting the publish output including `Avrdude\`); `Bundle.wixproj` chains the .NET 10 Desktop Runtime (`DotNetCoreSearch` + remote `ExePackage`) and produces the Burn bootstrapper `bin/Release/Fcc3ControllerSetup.exe`. The runtime version/URL/SHA-512 live in `Bundle.wxs`; see `docs/migration/installer-notes.md`.
- `FccInstaller/FccInstaller.vdproj` is legacy and is not built; `.vdproj` projects require Visual Studio + the Installer Projects extension.
- C# is the default for `net10.0` (C# 14); the old explicit `LangVersion 7.3` props were removed. `*.Designer.cs` / `*.resx` are designer-generated.

## Cross-component contracts (change together)

- USB IDs: VID `0x1029`, PID `0xF16C`. `FccHandeler.Connect()` requires exactly one matching HID device.
- HID IN report (endpoint size 12) is bit-packed: 11-bit X/Y, 4-bit hat, 19-bit buttons, then options and a 16-bit user-force value. `FccHandeler.Update()` decodes it manually (X/Y from buffer bytes 1–3, options at byte 7, force at bytes 8–9). The authoritative layout spans `USB_JoystickReport_Data_t` (`Joystick.h`), the report descriptor in `Descriptors.c`, and that decoder — change all three together.
- HID OUT report is 3 bytes: `[options bitmask][force/command int16 LE]`. Commands are set in the force field's high bits: `Center = 0x4000`, `Reboot = 0x8000`.
- The options bitmask is duplicated in `fcc3.h` (`enum ConfigOptions`) and `FccHandeler.ConfigOptions`; keep values identical (`0x01` rotated sensors, `0x02/0x04` analog/digital FLCS, `0x10/0x20/0x40/0x80` force presets).
- Firmware version shown by the app comes from `ReleaseNumber = VERSION_BCD(...)` in `Descriptors.c`; app version comes from `AssemblyInfo.cs`.

## Hardware / runtime quirks

- `fcc3.h` defines `LOCAL_ADC`, selecting the ATmega internal ADC (channels 4/5, 10-bit). Undefining it switches to the external MCP3202 over SPI (12-bit) — the two paths are not compiled together, and scaling constants differ.
- Axis centering is measured at every boot (`ReadStickZero`), not persisted. Only options, user force, and rotation sin/cos live in EEPROM; empty bytes (0/0xFF) fall back to defaults.
- EEPROM is written only by `exitConfig()` / `processStickOut()` (`WriteMem`), and only changed values are updated.
- Grip config mode: hold the grip config combo for 3 s; LED pulses, buttons are suppressed, 15 s inactivity exits and saves. Grip button bits are remapped to Windows HID bits in `ReadGrip()`.
- Firmware update path: `Reboot` command → `RebootToBootloader()` writes magic `0x7777` to RAM `0x0800`, detaches USB, watchdog-resets into the Caterina/Arduino bootloader. The app waits for a new COM port and runs bundled `Avrdude\avrdude.exe -cavr109 -b57600` (AVRDUDE 8.3 x64, statically linked — no libusb DLL needed; `avrdude.conf` is trimmed to the ATmega32U4 part chain plus the `avr109`/`avr911` programmer), so `Avrdude/` must ship next to the executable and the board needs an Arduino-compatible bootloader.

## Release packaging gotcha

`GithubUpdater` reads the latest release of `uriba107/fcc_controller` (hardcoded in `HelperFunctions.cs`): firmware is the first asset containing `.hex`; the app prefers a `fcc_controller_*` `.exe` Burn bundle and falls back to `.msi` for older releases. Asset names are parsed as `fcc_controller_<version>` + extension, so uploads must be renamed to `fcc_controller_<version>.hex`, `fcc_controller_<version>.exe` (local output `Fcc3ControllerSetup.exe`) and/or `fcc_controller_<version>.msi` (local output `Fcc3Controller.msi`). A release missing one component's assets no longer crashes the updater (`FirstOrDefault` + null guards in `GithubUpdater.cs`), but missing assets show as 0.0.0. Missing `FirmwareFileName`/`AppFileName` returns null, so download buttons stay disabled. Bump the version in `Descriptors.c` (firmware) and `AssemblyInfo.cs` plus `ProductVersion` in `Installer.wixproj`/`Bundle.wixproj` (app); releases are cut via the workflows' dispatch buttons and the `fw-v`/`app-v` tags are created by CI.

## Working-tree caution

The net48 → .NET 10 migration lives on the `migration/target-net10` branch (see `docs/migration/`). `git status` may show in-progress migration edits under `src/App`; treat them as user work and inspect before editing or reverting. The HidSharp 1.5→2.6.4 / net48 upgrade is already committed on `develop`.
