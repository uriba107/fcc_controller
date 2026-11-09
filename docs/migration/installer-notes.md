# Installer migration notes (net48 → .NET 10) — input for MO4

Research only. No project/installer file was modified. Everything below is for the
hands-on MO4 session (`src/App/WindowsInstaller/WindowsInstaller.vdproj`, edited in
Visual Studio with the Installer Projects extension).

Target: `net10.0-windows`, **framework-dependent** MSI + .NET 10 Desktop Runtime
chained prerequisite (plan §Decision Gates). Self-contained is the documented fallback.

---

## 0. What the current vdproj actually contains

File: `src/App/WindowsInstaller/WindowsInstaller.vdproj` (869 lines, added in `fa7e451`).
Read it before editing; the relevant entries are:

| Area | Current value | MO4 action |
|------|---------------|------------|
| `Configurations/Debug\|Release` → `InstallerBootstrapper` | `3:2` (bootstrapper enabled) | keep |
| `BootstrapperCfg:{...}` blocks | **absent** → no prerequisites selected | add (§1) |
| `ExternalPersistence/LaunchCondition` | `.NET Framework`, `FrameworkVersion = .NETFramework,Version=v4.7.2` | replace with .NET Core condition (§4) |
| `File` section | 4 × `IsDependency = TRUE`: `System.Net.Http.dll`, `HidSharp.dll` (2.1.0.0), `GithubUpdater.dll`, `HexUploader.dll` | delete (stale) (§2.2) |
| `Folder` section | only Application Folder (`[ProgramFiles64Folder][Manufacturer]\[ProductName]` = `C:\Program Files\FCC Controller\Fcc Configurator`) | keep; Avrdude comes from Publish Items (§3) |
| `ProjectOutput` | `SourcePath = ..\Fcc3_configurator\obj\Publish\FCC_configurator.exe`, `OutputGroupCanonicalName = Built`, `OutputProjectGuid = {A9F58F2C-7D23-493F-A00C-374FD64EF23B}`, `ShowKeyOutput = TRUE` | switch to `PublishItems` (§2.1) |
| `Product` | `ProductVersion = 1.0.2`, `TargetPlatform = 1` (x64), `OutputFilename = Fcc3Controller.msi` | keep (version bump is out of scope) |

**Critical gap found:** the old `src/App/FccInstaller/FccInstaller.vdproj` had explicit `File`
entries for `..\HexUploader\Avrdude\avrdude.exe`, `avrdude.conf`, `libusb0.dll` and an
`Avrdude` subfolder. The new `WindowsInstaller.vdproj` **dropped all of them**. With the
current `Built` output group, Primary Output does **not** include content files, so the
committed vdproj does **not** package Avrdude despite the commit message. MO4 must fix
this (§3).

---

## 1. .NET 10 Desktop Runtime as a bootstrapper prerequisite

### 1.1 Exact BootstrapperPackage entry / product code

| Item | x64 | x86 |
|------|-----|-----|
| Bootstrapper package folder | `net10desktopruntime_x64` | `net10desktopruntime_x86` |
| `ProductCode` (bootstrapper package) | `Microsoft.NetCore.DesktopRuntime.10.0.x64` | `Microsoft.NetCore.DesktopRuntime.10.0.x86` |
| Runtime installer ARP ProductCode | `{29A18B06-B190-4463-9FFC-BC0D70D80B7C}` | `{7349986C-C841-477D-A251-61FA8DD38D63}` |
| Runtime installer file | `windowsdesktop-runtime-10.0.14-win-x64.exe` | `windowsdesktop-runtime-10.0.14-win-x86.exe` |
| Download URL | `https://download.microsoft.com/download/9b53b153-e93a-4c89-8049-c7381505cf80/8de343c4-257a-40b1-9af3-3d7782328bc5/windowsdesktop-runtime-10.0.14-win-x64.exe` | `https://download.microsoft.com/download/9b53b153-e93a-4c89-8049-c7381505cf80/b6927599-c55d-49aa-aeda-2645116ed05e/windowsdesktop-runtime-10.0.14-win-x86.exe` |
| SHA-256 | `91C2795A7B1E2480CAA83FCE9514DC2E2D21E277D9A297311FABA6040BAD7330` | `DC55074AB269C4C13AA51D684EF803D4B5AD5EE5E1EB8238EFC190C8FBD0D5FD` |

The bootstrapper package `ProductCode` follows the 5.0→9.0 pattern
(`Microsoft.NetCore.DesktopRuntime.<major>.<minor>.x64`) and must match the
`product.xml` in the package folder. The runtime installer ARP ProductCode is the
WiX Burn bundle code (what Add/Remove Programs shows), **not** the bootstrapper
package code — do not confuse them.

`.vdproj` entry to add under **both** `Debug` and `Release` configurations (VS writes
this automatically when you tick the prerequisite; shown here for hand-editing /
verification):

```text
"BootstrapperCfg:{63ACBE69-63AA-4F98-B2B6-99F9E24495F2}"
{
"Enabled" = "11:TRUE"
"PromptEnabled" = "11:TRUE"
"PrerequisitesLocation" = "2:1"
"Url" = "8:"
"ComponentsUrl" = "8:"
    "Items"
    {
        "{EDC2488A-8267-493A-A98E-7D9C3B36CDF3}:Microsoft.NetCore.DesktopRuntime.10.0.x64"
        {
        "Name" = "8:.NET Desktop Runtime 10.0.14 (x64)"
        "ProductCode" = "8:Microsoft.NetCore.DesktopRuntime.10.0.x64"
        }
    }
}
```

* `{63ACBE69-63AA-4F98-B2B6-99F9E24495F2}` is the fixed bootstrapper-config key.
* `{EDC2488A-8267-493A-A98E-7D9C3B36CDF3}` is the fixed `Items` key used for every
  bootstrapper package (same GUID appears for .NET Framework and .NET Core entries).
* `PrerequisitesLocation = "2:1"` is the value VS writes by default and is what the
  existing FccInstaller/PeperNote vdproj files use for the offline “same location as my
  application” choice. Let VS write it; only change it deliberately.
* `Name` must match the `package.xml` `DisplayName` of the package version
  (`.NET Desktop Runtime 10.0.14 (x64)` for 10.0.14).

Version note: 10.0.14 is the latest .NET 10 Desktop Runtime patch as of 2026-11 and is
the runtime bundled with SDK 10.0.403. Framework-dependent roll-forward is within the
same major.minor, so a machine with a newer 10.0.x satisfies the check; a machine with
an older 10.0.x does not. Keep the prerequisite patch level in sync with the SDK used
to build/release.

### 1.2 How VS exposes it

1. Open `src/App/Fcc3_configurator.sln` in Visual Studio (Installer Projects extension
   installed).
2. Select the `WindowsInstaller` project → **Project → Properties → Prerequisites…**.
3. Tick **Create setup program to install prerequisite components**.
4. Tick **.NET Desktop Runtime 10.0.x (x64)** (WinForms → Desktop Runtime, not
   “.NET Core Runtime”).
5. Choose the install location (offline “same location as my application” vs online
   “vendor's web site”).
6. OK → rebuild. VS writes the `BootstrapperCfg` block and the .NET Core launch
   condition.

The dialog is populated from:

* `<VSInstall>\MSBuild\Microsoft\VisualStudio\BootstrapperPackages` (VS 2019 Update 7+), and
* `%ProgramFiles(x86)%\Microsoft SDKs\ClickOnce Bootstrapper\Packages`

(also readable from `HKLM\Software\Wow6432Node\Microsoft\GenericBootstrapper` → `Path`).

### 1.3 Does the CI runner support .NET 10? (important)

**Not by default.** Evidence:

* `windows-latest` = `windows-2025` image: VS 2022 **17.14.37827.9**, Installer
  Projects extension **3.0.0** (`Component.VSInstallerProjects2022` = 3.0.0).
* The extension does **not** ship bootstrapper packages: the 3.0.0 VSIX contains no
  `product.xml`/`package.xml` (verified by extracting the VSIX).
* The runner's VS 2022 components include `Microsoft.NetCore.Component.Runtime.8.0`
  and `…Runtime.9.0` but **no `…Runtime.10.0`**; `Microsoft.Component.ClickOnce` is
  17.14.36510.44. The .NET 10 SDKs on the runner are installed separately via
  `dotnet-install` zip (`Install-DotnetSDK.ps1`), which does not add bootstrapper
  packages.
* Microsoft's .NET SDK/VS table: **.NET 10 requires Visual Studio 2026 18.0+**; VS 2022
  17.14 supports up to .NET 9.

So on the runner the Prerequisites dialog will **not** list “.NET Desktop Runtime
10.0.x”. The extension version (3.0.0) is not the limiter — the VS installation's
ClickOnce/bootstrapper packages are. Options for MO4:

1. **Preferred:** perform MO4 on a machine with **VS 2026 (18.x)**, where
   `net10desktopruntime_x64` is present.
2. **Manual package:** copy `net10desktopruntime_x64` from a VS 2026 install, or create
   it (template in §1.4), into
   `C:\Program Files\Microsoft Visual Studio\2022\<edition>\MSBuild\Microsoft\VisualStudio\BootstrapperPackages\`.
   Admin rights required (the CI runner has them; a dev machine usually does). Then
   reopen the Prerequisites dialog — the entry appears.
3. **Self-contained fallback:** publish with `<SelfContained>true</SelfContained>` +
   `RuntimeIdentifier=win-x64` and skip the prerequisite. MSI grows to ~150 MB. This
   is the plan's documented fallback, not the default.

For MO5 (CI): if the MSI is built on the runner, either script option 2 before the
`devenv` build or build the MSI on a VS 2026 runner/self-hosted agent. The current
`app.yml` builds the vdproj with `continue-on-error: true`, so a missing prerequisite
fails silently — do not rely on that.

### 1.4 Manual `net10desktopruntime_x64` template (only if no VS 2026 available)

`product.xml`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Product xmlns="http://schemas.microsoft.com/developer/2004/01/bootstrapper"
         ProductCode="Microsoft.NetCore.DesktopRuntime.10.0.x64">
  <PackageFiles CopyAllPackageFiles="false">
    <PackageFile Name="windowsdesktop-runtime-10.0.14-win-x64.exe"
                 HomeSite="https://download.microsoft.com/download/9b53b153-e93a-4c89-8049-c7381505cf80/8de343c4-257a-40b1-9af3-3d7782328bc5/windowsdesktop-runtime-10.0.14-win-x64.exe" />
    <PackageFile Name="NetCoreCheck.exe" />
  </PackageFiles>
  <Commands>
    <Command PackageFile="windowsdesktop-runtime-10.0.14-win-x64.exe" Arguments=" /quiet /norestart">
      <InstallConditions>
        <BypassIf Property="DotNet10DesktopRuntimeInstalled" Compare="ValueGreaterThanOrEqualTo" Value="10.0.14" />
      </InstallConditions>
      <ExitCodes>
        <ExitCode Value="0" Result="Success" />
        <ExitCode Value="3010" Result="SuccessReboot" />
        <ExitCode Value="1641" Result="SuccessReboot" />
        <ExitCode Value="1603" Result="Fail" />
      </ExitCodes>
    </Command>
  </Commands>
</Product>
```

`en\package.xml`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Package xmlns="http://schemas.microsoft.com/developer/2003/01/bootstrapper"
         Name="DisplayName" Culture="en">
  <Strings>
    <String Name="DisplayName">.NET Desktop Runtime 10.0.14 (x64)</String>
    <String Name="Culture">en</String>
  </Strings>
</Package>
```

Caveats:

* `DotNet10DesktopRuntimeInstalled` is produced by `NetCoreCheck.exe`. A minimal
  `product.xml` without the correct `<InstallChecks>` cannot detect an existing
  runtime, so the bootstrapper will always reinstall. **Copying the complete
  `net10desktopruntime_x64` folder from a VS 2026 install is strongly preferred.**
* For “same location as my application”, the runtime `.exe` must be present in the
  package folder at build time. For “vendor's web site”, `HomeSite` is used and
  `PublicKey` is required (use the Microsoft certificate public key from the real
  package; `PublicKey="0"` causes build warnings).
* x86 template: replace `x64`→`x86`, `10.0.x64`→`10.0.x86`, and the file/URL. The app
  is x64 (`TargetPlatform = 1`), so x64 is the one that matters.

---

## 2. Align the vdproj with the SDK-style output layout

### 2.1 Use **Publish Items**, not Primary Output

Microsoft's Installer Projects + .NET guidance: for .NET Core 3.1 / .NET 5+ projects,
select **Publish Items** instead of Primary Output. The .NET SDK target
`PublishItemsOutputGroup` is explicitly documented as “Emit an output group containing
all files that get published. **This will be consumed by VS installer projects.**”

* The canonical name written to the vdproj is **`PublishItems`** (real-world examples:
  `"OutputGroupCanonicalName" = "8:PublishItems"`).
* The old `Built` group is Primary Output and points at
  `..\Fcc3_configurator\obj\Publish\FCC_configurator.exe`. It does not represent the
  SDK-style publish layout (`bin\Release\net10.0-windows\`).

UI steps (preferred over hand-editing):

1. Setup project → **File System** editor → right-click **Application Folder** →
   **Add → Project Output…**
2. Select **FCC_configurator** → choose **Publish Items** → Configuration **Release**
   (or Active) → OK.
3. Delete the old **Primary output from FCC_configurator (Built)** node.

Resulting `ProjectOutput` entry (VS regenerates the key GUID and `SourcePath`; the
canonical name is the load-bearing value):

```text
"{5259A561-127C-4D43-A0A1-72F10C7B3BF8}:_DAF0443DAEBD435B8D14925BA0FF31E2"
{
"SourcePath" = "8:..\\Fcc3_configurator\\obj\\Release\\net10.0-windows\\FCC_configurator.exe"
"TargetName" = "8:"
"Tag" = "8:"
"Folder" = "8:_3A62B3896E6545F8BB4AEF0498AE22E7"
...
"OutputGroupCanonicalName" = "8:PublishItems"
"OutputProjectGuid" = "8:{A9F58F2C-7D23-493F-A00C-374FD64EF23B}"
"ShowKeyOutput" = "11:TRUE"
}
```

`SourcePath` will be under `obj\Release\net10.0-windows\` for SDK-style projects
(`apphost.exe`/`FCC_configurator.exe`); VS updates it on build. Do not leave the stale
`obj\Publish\` path.

Before building the MSI, publish the app (framework-dependent):

```powershell
dotnet publish src/App/Fcc3_configurator -c Release -f net10.0-windows
```

### 2.2 Detected Dependencies (`File` section)

The four `IsDependency = TRUE` entries are stale after the migration:

| Entry | Why stale |
|-------|-----------|
| `System.Net.Http.dll` | .NET Framework assembly; System.Net.Http is in the .NET 10 shared framework. Remove. |
| `HidSharp.dll` | Version changes to 2.6.4.0. Remove; the current DLL is in the publish output. |
| `GithubUpdater.dll` | Still needed, but included via Publish Items. Remove the old entry. |
| `HexUploader.dll` | Still needed, but included via Publish Items. Remove the old entry. |

**Recommended:** in the File System editor, right-click each → **Delete**. The
corresponding `Hierarchy` entries are removed automatically. Do not hand-edit the
`File`/`Hierarchy` sections unless you also fix every `MsmKey`/`OwnerKey` reference.

After deletion, the dependency DLLs (`HidSharp.dll`, `GithubUpdater.dll`,
`HexUploader.dll`, `WinForms.DataVisualization.dll`,
`System.Configuration.ConfigurationManager.dll`, `System.IO.Ports.dll`, …) come from
the Publish Items group. Verify the installed layout (§3.2).

### 2.3 File System entries

With Publish Items, the Application Folder receives the entire publish output,
including subfolders. **No extra File System entries are needed for Avrdude** — the
publish output preserves the relative path `Avrdude\…`.

If Primary Output is kept (not recommended), Avrdude must be added explicitly (§3.1).

### 2.4 Framework-dependent: do not bundle the runtime

Decision Gate 2 is framework-dependent. Ensure the publish is framework-dependent:

* `<SelfContained>` is `false` (default) and no `RuntimeIdentifier` is set (or
  `win-x64` with `<SelfContained>false</SelfContained>`).
* The Publish Items group then contains only the app's files; the .NET 10 Desktop
  Runtime is installed by `Setup.exe` **before** the MSI.

If `SelfContained=true`, the Publish Items group includes the whole runtime (~150 MB)
and the MSI bundles it — that is the fallback, not the plan.

---

## 3. Avrdude + HexHelper content

### 3.1 How the content flows (and the current gap)

`src/App/HexUploader/HexHelper.csproj` has:

```xml
<None Include="Avrdude\avrdude.conf">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</None>
<Content Include="Avrdude\avrdude.exe">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</Content>
<Content Include="Avrdude\libusb0.dll">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</Content>
```

S-C keeps these items exact. They flow transitively to `FCC_configurator`'s output and
publish folder via MSBuild `GetCopyToOutputDirectoryItems`, so the app's publish folder
contains `Avrdude\avrdude.exe`, `Avrdude\avrdude.conf`, `Avrdude\libusb0.dll`.

The vdproj picks them up **only if** you use Publish Items (or add the `ContentFiles`
output group / explicit file entries). Primary Output (`Built`) does **not** include
content files. The current vdproj has neither Publish Items nor explicit Avrdude
entries → **Avrdude is missing**.

Fix path A (recommended): switch to **Publish Items** (§2.1). Avrdude is included
automatically.

Fix path B (if keeping Primary Output): add a second `ProjectOutput` node for the
HexHelper project with `"OutputGroupCanonicalName" = "8:ContentFiles"`, or explicit
`File` entries plus an `Avrdude` subfolder (copy the pattern from the old
`FccInstaller.vdproj`):

```text
"{1FB2D0AE-D3B9-43D4-B9DD-F88EC61E35DE}:_<guid>"
{
"SourcePath" = "8:..\\HexUploader\\Avrdude\\avrdude.exe"
"TargetName" = "8:avrdude.exe"
"Folder" = "8:_<AvrdudeFolderKey>"
...
}
```

and similarly for `avrdude.conf` and `libusb0.dll`, with an `Avrdude` folder under the
Application Folder.

### 3.2 Verify the installed layout

Extract the MSI without installing (administrative install):

```powershell
msiexec /a "<path>\Fcc3Controller.msi" /qn TARGETDIR="<empty-dir>"
```

(or use `lessmsi` / 7-Zip to list the MSI). Check:

* `<dir>\FCC_configurator.exe`
* `<dir>\FCC_configurator.dll`, `.deps.json`, `.runtimeconfig.json`
* `<dir>\HidSharp.dll`, `GithubUpdater.dll`, `HexUploader.dll`,
  `WinForms.DataVisualization.dll`, `System.Configuration.ConfigurationManager.dll`,
  `System.IO.Ports.dll` (as applicable)
* `<dir>\Avrdude\avrdude.exe`
* `<dir>\Avrdude\avrdude.conf`
* `<dir>\Avrdude\libusb0.dll`

Or install on a VM and check
`C:\Program Files\FCC Controller\Fcc Configurator\Avrdude\`.

Pre-MSI check: confirm `src\App\Fcc3_configurator\bin\Release\net10.0-windows\publish\Avrdude\`
exists after `dotnet publish`.

---

## 4. Launch condition: replace .NET Framework 4.7.2 with .NET Core

Current entry to remove/replace:

```text
"ExternalPersistence"
{
"LaunchCondition"
{
"{A06ECF26-33A3-4562-8140-9B0E340D4F24}:_FBB91D4136F442049F2039187641B040"
{
"Name" = "8:.NET Framework"
"Message" = "8:[VSDNETMSG]"
"FrameworkVersion" = "8:.NETFramework,Version=v4.7.2"
"AllowLaterVersions" = "11:FALSE"
"InstallUrl" = "8:http://go.microsoft.com/fwlink/?LinkId=863262"
}
}
}
```

Replace with the .NET Core condition (VS generates this when the .NET Desktop Runtime
prerequisite is selected):

```text
"ExternalPersistence"
{
"LaunchCondition"
{
"{A06ECF26-33A3-4562-8140-9B0E340D4F24}:_<new-guid>"
{
"Name" = "8:.NET Core"
"Message" = "8:[VSDNETCOREMSG]"
"AllowLaterVersions" = "11:FALSE"
"InstallUrl" = "8:https://dotnet.microsoft.com/download/dotnet-core/[NetCoreVerMajorDotMinor]"
"IsNETCore" = "11:TRUE"
"Architecture" = "2:0"
"Runtime" = "2:0"
}
}
}
```

* `Architecture = 2:0` and `Runtime = 2:0` are the values VS writes for a .NET Desktop
  Runtime **x64** prerequisite (copied from a working .NET 9 vdproj). Let VS assign the
  key GUID; do not invent these integers.
* The launch condition is only a fallback; `Setup.exe` installs the runtime first.

---

## 5. MO4 step-by-step checklist

1. **Toolchain:** VS 2026 (preferred) or VS 2022 17.14 + Installer Projects 3.0.0 +
   manually added `net10desktopruntime_x64` package. Confirm the Prerequisites dialog
   lists “.NET Desktop Runtime 10.0.14 (x64)”.
2. **Publish the app** (framework-dependent):
   `dotnet publish src/App/Fcc3_configurator -c Release -f net10.0-windows`.
   Confirm `bin\Release\net10.0-windows\publish\Avrdude\` exists.
3. Open `src/App/Fcc3_configurator.sln` in VS; let the SDK-style projects load.
4. **File System editor:**
   * Delete the four detected dependencies (System.Net.Http, HidSharp 2.1.0.0,
     GithubUpdater, HexUploader).
   * Delete “Primary output from FCC_configurator (Built)”.
   * Add → Project Output → FCC_configurator → **Publish Items** → Release.
   * Confirm the Application Folder shows `Avrdude\`.
5. **Project → Properties → Prerequisites…:**
   * Tick “Create setup program to install prerequisite components”.
   * Tick “.NET Desktop Runtime 10.0.14 (x64)”.
   * Choose offline (“same location as my application”, runtime exe present in the
     package folder) or online (“vendor's web site”).
   * OK.
6. Confirm the .NET Framework launch condition was replaced by the .NET Core condition
   (`IsNETCore = TRUE`).
7. **Build** the WindowsInstaller project (Release). Output:
   `src\App\WindowsInstaller\Release\Fcc3Controller.msi`.
8. **Verify the MSI** (§3.2): exe, deps, `Avrdude\avrdude.exe`, `avrdude.conf`,
   `libusb0.dll`.
9. **Clean-VM test:** run `Setup.exe` (not the MSI) on a machine without the .NET 10
   Desktop Runtime. Confirm it installs the runtime, then the app; the app launches and
   the firmware-upgrade path finds `Avrdude\avrdude.exe`.
10. Smoke-test the installed app (Main, StickTracker, Advanced, FwUpgrade layout).
11. Record the exact vdproj diff and MSI SHA256 in the MO4 notes. **Do not bump**
    `ProductVersion` (version bump is out of scope for this migration).

---

## 6. Rollback

* The vdproj is one text file: `git checkout -- src/App/WindowsInstaller/WindowsInstaller.vdproj`
  reverts all MO4 edits. Baseline commit: `fa7e451`.
* The manually added `net10desktopruntime_x64` folder lives under the VS install
  (outside the repo). Delete it to roll back the build machine.
* MSI output (`src/App/WindowsInstaller/Release/Fcc3Controller.msi`) is gitignored;
  delete it so the CI artifact staging does not pick up a stale MSI.
* No project file, csproj, or firmware file is touched by MO4.

---

## 7. Sources

* Visual Studio Installer Projects and .NET (Publish Items, Prerequisites dialog):
  https://learn.microsoft.com/en-us/visualstudio/deployment/installer-projects-net-core
* Create bootstrapper packages (package folder locations, product/package manifests):
  https://learn.microsoft.com/en-us/visualstudio/deployment/creating-bootstrapper-packages
* How to include prerequisites with a ClickOnce application (BootstrapperPackages path):
  https://learn.microsoft.com/en-us/visualstudio/deployment/how-to-include-prerequisites-with-a-clickonce-application
* .NET 10 download page (Desktop Runtime x64/x86):
  https://dotnet.microsoft.com/en-us/download/dotnet/10.0
* winget manifest for 10.0.14 (download URLs, SHA-256, ARP ProductCodes):
  https://github.com/microsoft/winget-pkgs/blob/master/manifests/m/Microsoft/DotNet/DesktopRuntime/10/10.0.14/Microsoft.DotNet.DesktopRuntime.10.installer.yaml
* Real vdproj with `Microsoft.NetCore.DesktopRuntime.9.0.x64` BootstrapperCfg + .NET
  Core launch condition (PeperNote):
  https://github.com/jpmikkers/PeperNote/blob/7ba7ed89/PeperNoteSetup/PeperNoteSetup.vdproj
* Real vdproj with `"OutputGroupCanonicalName" = "PublishItems"`:
  https://github.com/cooldope/Crypto-Wallet-Stealer-2024/blob/main/Crypto%20Wallet%20Stealer%20v2.3.vdproj
* .NET SDK `PublishItemsOutputGroup` target (“consumed by VS installer projects”):
  https://github.com/dotnet/sdk/blob/main/src/Tasks/Microsoft.NET.Build.Tasks/targets/Microsoft.NET.Publish.targets
* GitHub runner image (VS 2022 17.14.37827.9, Installer Projects 3.0.0, ClickOnce
  17.14.36510.44, .NET runtime components 8.0/9.0 only):
  https://github.com/actions/runner-images/blob/main/images/windows/Windows2025-Readme.md
* Microsoft .NET SDK / Visual Studio version table (.NET 10 → VS 2026 18.0+):
  https://learn.microsoft.com/en-us/dotnet/core/install/windows
* Developer Community: “.Net 10.0.1 not available in Installer Bootstrapper packages”
  (evidence .NET 10 was absent from VS 2022 bootstrapper packages):
  https://developercommunity.visualstudio.com/t/11018229
* Visual Studio Installer Projects 2022 Marketplace (3.0.0, works with VS 2022/2026):
  https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2022InstallerProjects
