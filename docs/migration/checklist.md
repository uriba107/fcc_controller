# FCC3 Configurator — net48 → .NET 10 Migration: Behavioral Acceptance Checklist

**Purpose.** This sheet is the executable behavioral acceptance gate for the net48 → .NET 10
migration (see `docs/migration/net10-upgrade-plan.md`, verification task **V‑1**). It verifies
that the net10 build preserves the observable behavior of the net48 build for the
cross-component contracts documented in the repo `AGENTS.md` and implemented in `src/App/**`
and `src/Controller/Fcc3/**`.

**How to run.**

1. Build the net48 baseline (`FCC_configurator.exe`, assembly version **1.0.2.0**) and the
   net10 build. Keep both output folders, each with `Avrdude\` next to the executable.
2. Run both builds side by side on the same Windows 10/11 machine with the FCC3 device
   attached. Use the same physical inputs for both.
3. Work through the table top to bottom. For each row, perform the **Steps**, compare against
   **Expected**, and write `pass` or `fail` in the **Result** column. Record observations,
   captured bytes, or screenshots in **Notes**.
4. Rows marked **★** are code-verified where a second physical device, a USB capture tool, or a
   controlled GitHub release is unavailable. State the evidence used in **Notes**.
5. Mark a row `fail` if net10 differs from net48 in any observable way, even when the new
   behavior looks "better". Regression parity is the gate, not improvement.

**Prerequisites.**

- FCC3 USB adapter (VID `0x1029`, PID `0xF16C`) with grip and stick.
- net48 build output and net10 build output, each with `Avrdude\avrdude.exe` and
  `Avrdude\avrdude.conf` beside the executable.
- Internet access for the GithubUpdater checks.
- A firmware `.hex` for the update check (or use the auto-download path).

**Cross-component contracts under test** (from repo `AGENTS.md`):

- USB IDs: VID `0x1029`, PID `0xF16C`; `FccHandeler.Connect()` requires **exactly one**
  matching HID device.
- HID IN report (endpoint size 12; the HidSharp buffer includes a 1-byte report ID at index 0):
  11-bit X/Y from buffer bytes 1–3, 4-bit hat, 19-bit buttons, options at byte 7, 16-bit
  little-endian user force at bytes 8–9.
- HID OUT report: 3 bytes `[options bitmask][force/command int16 LE]` (the HidSharp buffer has
  a leading report-ID byte `0x00`). Commands: `Center = 0x4000`, `Reboot = 0x8000`.
- Options bitmask duplicated in `fcc3.h` (`enum ConfigOptions`) and `FccHandeler.ConfigOptions`.
- Firmware update: `Reboot` command → bootloader → new COM port →
  `Avrdude\avrdude.exe -cavr109 -b57600`.

**Known HID IN reports** (byte 0 is the HidSharp report ID; use these to verify the decode):

| Report bytes (hex)        | X    | Y   | Options       | Force    |
|---------------------------|------|-----|---------------|----------|
| `00 64 70 3E 00 00 00 01 00 00` | 100  | −50 | `0x01` RotatedSensors | `0x0000` |
| `00 00 00 00 00 00 00 12 34 12` | 0    | 0   | `0x12` Force4Kg + AnalogFlcs | `0x1234` |

Decode contract used above: `X = buff[1] | (buff[2] & 0x07) << 8`;
`Y = (buff[2] >> 3) | (buff[3] & 0x3F) << 5`; `options = buff[7]`;
`force = buff[8] | buff[9] << 8`.

---

## Acceptance table

| # | Check | Steps | Expected | Result (pass/fail) | Notes | Tester | Date |
|---|-------|-------|----------|-------------------|-------|--------|------|
| 1 | **Device connect** — exactly one HID device, VID `0x1029` / PID `0xF16C` | 1. With no FCC3 device attached, launch the app; wait 3 s. 2. Record the status-bar text and the state of Test Stick / Center / Initial Setup. 3. Attach one FCC3 device; wait up to 3 s. 4. Record the status-bar text, the detected firmware version, and the button states. 5. Detach the device; wait 3 s and record the state. 6. Repeat on the other build. | No device: status bar reads **"No Device Connected"** (red); Test Stick, Center, and Initial Setup are disabled; no crash. One device: status bar reads **"FCC (Cougar)"** or **"FCC (Warthog) Connected"** (green); the firmware version (e.g. `1.0.7`) is shown; the buttons are enabled. Zero or multiple matching devices: `Connect()` returns `false` and the app stays in the no-device state (★ code-verified if only one device is available). | | `FccHandeler.Connect()` uses `DeviceList.Local.GetHidDevices(VID, PID)` and requires `Count() == 1`; the USB timer retries every 1 s (`HelperFunctions.cs`). | | |
| 2 | **HID IN decode parity** — X/Y (bytes 1–3, 11-bit), options (byte 7), user force (bytes 8–9) | 1. Connect the device. On the net48 build, open **Test Stick** and record X/Y at center, full-left, full-right, full-up, and full-down. 2. Record the options readback on the Main Form (FLCS mode, sensor rotation, force preset) and the user-force value (`labelCurrentUserDefined`) for a known custom force. 3. Repeat on the net10 build with the same physical inputs. 4. If a raw HID capture tool is available, capture a 10-byte input report and verify the app's decode against the known reports above; repeat on both builds. | net10 shows the same X/Y (within ±1 LSB), the same options bits, and the same user-force value as net48 for the same inputs. The known reports decode to the values in the table above. | | `buff[0]` is the HidSharp report ID (always 0 here). The app does not decode hat/buttons; that is covered by the raw-capture step if available. | | |
| 3 | **HID OUT report** — 3 bytes `[options][force LE]`; Center `0x4000`, Reboot `0x8000` | 1. With the device connected, click **Center** on the net48 build; observe the stick re-center. 2. Change an option (e.g. sensor rotation) and click **Apply**; observe the device behavior. 3. Trigger a firmware reboot from the app; observe the device enter the bootloader. 4. If a USB capture tool is available, capture the OUT reports and record the bytes. 5. Repeat on the net10 build. | Center sends force `0x4000` (options `0x00`). Apply sends the options bitmask followed by the requested force (little-endian). Reboot sends force `0x8000`. The device responds identically on both builds. | | `SendToStick` writes `[0][options][forceLo][forceHi]`; HidSharp strips the leading report-ID byte and sends the 3 report bytes. The firmware reads `Endpoint_Read_8()` then `Endpoint_Read_16_LE()`. | | |
| 4a | **Options bit 0x01 — RotatedSensors** | 1. On the net48 build, toggle **Rotate Sensors** and click **Apply**. 2. Record the Main Form sensor-rotation label and the stick behavior. 3. Repeat on the net10 build. | The label toggles between ON (green) and OFF (red) and the device applies the rotation; both builds agree. | | `fcc3.h`: `RotatedSensors = 0x01`; `FccHandeler.ConfigOptions`: `RotatedSensors = 0x01`. | | |
| 4b | **Options bit 0x02 — AnalogFlcs** | 1. On the net48 build, enable **Force Mapping**, select **Analog FLCS**, and click **Apply**. 2. Record the FLCS label. 3. Repeat on the net10 build. | The label reads **"FLCS: Analog"** (green); both builds agree. | | `fcc3.h`: `AnalogFlcs = 0x02`; app enum spells it `AnaloglFlcs = 0x02` (typo preserved). | | |
| 4c | **Options bit 0x04 — DigitalFlcs** | 1. On the net48 build, enable **Force Mapping**, select **Digital FLCS**, and click **Apply**. 2. Record the FLCS label. 3. Repeat on the net10 build. | The label reads **"FLCS: Digital"** (green); both builds agree. | | `fcc3.h`: `DigitalFlcs = 0x04`; app: `DigitalFlcs = 0x04`. | | |
| 4d | **Options bit 0x08 — FccWhGains** | 1. On the net48 build, open **Initial Setup** and change the FCC revision (Cougar ↔ Warthog). 2. Record the status-bar text and the custom-force limits. 3. Repeat on the net10 build. | The status bar switches between **"FCC (Cougar)"** and **"FCC (Warthog)"**; the custom-force limits change (12 kgf vs 9 kgf); both builds agree. | | `fcc3.h`: `FccWhGains = 0x08`; app: `FccWhGains = 0x08`. Saved as `UseNewFccGain`. | | |
| 4e | **Options bit 0x10 — Force4Kg** | 1. On the net48 build, select the **4 kg/f** preset and click **Apply**. 2. Record the visible force label and the device behavior. 3. Repeat on the net10 build. | The `4 kg/f` label is shown and the device applies the preset; both builds agree. | | `fcc3.h`: `Force4Kg = 0x10`; app: `Force4Kg = 0x10`. | | |
| 4f | **Options bit 0x20 — Force6Kg** | 1. On the net48 build, select the **6 kg/f** preset and click **Apply**. 2. Record the visible force label and the device behavior. 3. Repeat on the net10 build. | The `6 kg/f` label is shown and the device applies the preset; both builds agree. | | `fcc3.h`: `Force6Kg = 0x20`; app: `Force6Kg = 0x20`. | | |
| 4g | **Options bit 0x40 — Force9Kg** | 1. On the net48 build, select the **9 kg/f** preset and click **Apply**. 2. Record the visible force label and the device behavior. 3. Repeat on the net10 build. | The `9 kg/f` label is shown and the device applies the preset; both builds agree. | | `fcc3.h`: `Force9Kg = 0x40`; app: `Force9Kg = 0x40`. | | |
| 4h | **Options bit 0x80 — ForceUserDefined** | 1. On the net48 build, select **User Defined**, set a custom force, and click **Apply**. 2. Record the displayed force and the device behavior. 3. Repeat on the net10 build. | The user-defined force label is shown and the device applies the custom force; both builds agree. | | `fcc3.h`: `ForceUserDefined = 0x80`; app: `ForceUserDefined = 0x80`. | | |
| 5 | **Config mode** — 3 s hold, LED pulse, buttons suppressed, 15 s inactivity exit + save, persistence | 1. With the device connected and the app running, hold the grip config combo (`GripConfigMode = 0x18`) for 3 s. 2. Observe the device LED. 3. While in config mode, press grip buttons and verify they are suppressed. 4. Press **DMS-right** to toggle sensor rotation; record the app's rotation label. 5. Press **TMS-right** to select digital FLCS; record the app's FLCS label. 6. Wait 15 s with no input; verify config mode exits and saves. 7. Restart the app and verify the settings persisted. 8. Repeat on the other build. | LED pulses during config mode. Buttons are suppressed (no button/hat output; verify with a raw HID capture if available — the app itself does not display buttons/hat). DMS-right toggles rotation; TMS-right selects digital FLCS. After 15 s idle the mode exits and writes EEPROM. Settings survive an app restart. Both builds agree. | | Firmware behavior (`fcc3.c`: `FccSettings`, `exitConfig`, `WriteMem`). The app is the host; it sees the resulting options/force readback. `GripConfigMode = 0x18` = `GripPaddle \| GripMissleStep`. | | |
| 6 | **Firmware update flow** — Reboot → bootloader → new COM port → `avrdude -cavr109 -b57600`; `Avrdude\` next to the exe | 1. Confirm `Avrdude\avrdude.exe` and `Avrdude\avrdude.conf` exist beside the executable on both builds. 2. Connect the device and open the firmware-upgrade form (Advanced → Firmware Update, or the Update tab's auto-update). 3. Click the update button. 4. Observe the device reboot into the bootloader (LED/USB re-enumeration). 5. Observe a new COM port appear within 5 s. 6. Observe avrdude run and the final status message. 7. Repeat on the other build. | The device reboots to the bootloader, a new COM port appears within 5 s, avrdude runs with `-cavr109 -b57600`, and the status reads **"Upgrade OK"** / **"FW upgrade has finished successfuly"**. Both builds agree. | | `HexUpdater.AutoDetectNewPort` polls every 50 ms for up to 5 s; `uploadHex` runs `Avrdude\avrdude.exe -v -patmega32u4 -cavr109 -P<port> -b57600 -D -Uflash:w:<hex>:i -C Avrdude\avrdude.conf` and waits up to 15 s. | | |
| 7 | **GithubUpdater** — latest version shown, both asset buttons, missing asset → 0.0.0 + disabled, `fcc_controller_<version>` parsing | 1. Connect the device (firmware version shown). 2. Launch the app with internet access and open the **Update** tab. 3. Record the latest firmware version and latest app version labels. 4. Record the state of the firmware and app update buttons. 5. Verify the versions match the latest `uriba107/fcc_controller` release assets named `fcc_controller_<version>.hex` / `.msi`. 6. For the missing-asset case, either test against a release missing one asset or verify the null-guard code path (★). 7. Repeat on the other build. | The latest release version is shown for both components. Both update buttons are enabled when an update is available. A missing asset shows `0.0.0` (firmware) / `0.0.0.0` (app) and disables the corresponding button; no crash. Both builds agree. | | `GithubUpdater` reads `https://api.github.com/repos/uriba107/fcc_controller/releases/latest`; it picks the first asset containing `.hex` / `.msi` and parses the version from the name after removing `fcc_controller_` and the extension. | | |
| 8 | **Single-instance mutex** | 1. Launch the app. 2. Launch a second instance (run the executable again). 3. Record the message and the state of both instances. | The second instance shows **"only one instance at a time"** and exits; the first instance keeps running. | | `Program.cs` uses the named mutex `{2B1592A2-23E4-493D-90C2-1B6DDADCB221}`. | | |
| 9 | **Settings** — `user.config` in the new .NET profile location; saved defaults reset once; no crash | 1. Launch the net10 app. 2. Change settings: FCC revision (gains), force preset, custom force, unit, and both notify flags. 3. Close the app. 4. Locate the `user.config` file and record its full path. 5. Relaunch the app and verify the settings persisted. 6. Verify the first run after migration resets the saved UI defaults once (expected) and does not crash. 7. Compare the path and behavior with net48. | Settings persist across restart. `user.config` lands in the new .NET profile location (different from the net48 path `%LocalAppData%\Fcc3_configurator\FCC_configurator.exe_Url_<hash>\1.0.2.0\user.config`). The one-time reset of saved defaults is expected and does not crash. | | Settings keys: `gOptions`, `gUserDefinedForce`, `gForceUnitlbf`, `notifyApp`, `notifyFirmware`, `UseNewFccGain`. | | |
| 10 | **Firmware version display** — `ReleaseNumberBcd` → `BdcDecode` | 1. Connect the device. 2. Record the firmware version shown on the Main Form and the Update tab. 3. Compare with the firmware's `VERSION_BCD(...)` in `Descriptors.c`. 4. Repeat on the other build. | The shown version matches the firmware (e.g. `1.0.7`) and both builds agree. | | `FccHandeler.BdcDecode` decodes `device.ReleaseNumberBcd` as `Major.Minor.Revision`. | | |

---

## Sign-off

| Item | Value |
|------|-------|
| net48 build version | |
| net10 build version | |
| Device firmware version | |
| Windows version | |
| Tester | |
| Date | |
| Overall result | ☐ pass ☐ fail |
| Blocking issues | |
| Signature | |

**Result column legend:** `pass` = net10 behavior matches net48 for this check; `fail` = any
observable difference or crash. Leave the Result, Tester, and Date cells blank while running;
fill them in as each row is completed.
