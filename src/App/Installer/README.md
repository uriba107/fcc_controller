# FCC3 Configurator installer (WiX v5)

Framework-dependent Windows installer, built with the WiX Toolset v5 SDK
(`WixToolset.Sdk` 5.0.2, restored from NuGet). No Visual Studio, Installer
Projects extension or globally installed WiX binary is required.

| Project | Output | Purpose |
|---------|--------|---------|
| `Installer.wixproj` (`Product.wxs`) | `bin/Release/<culture>/Fcc3Controller.msi` | x64 per-machine MSI; harvests the `dotnet publish` output (including `Avrdude\`) |
| `Bundle.wixproj` (`Bundle.wxs`) | `bin/Release/Fcc3ControllerSetup.exe` | Burn bootstrapper; detects/installs the .NET 10 Desktop Runtime, then runs the MSI |

## Build

```powershell
# from the repo root
dotnet publish src/App/Fcc3_configurator/FCC_configurator.csproj -c Release
dotnet build src/App/Installer/Bundle.wixproj -c Release
```

`Bundle.wixproj` has a `ProjectReference` to `Installer.wixproj`, so the MSI is
built first. To build only the MSI:

```powershell
dotnet build src/App/Installer/Installer.wixproj -c Release
```

Both projects share this folder, so `Directory.Build.props` gives each its own
`obj/<ProjectName>/` tree to avoid NuGet restore-asset collisions.

## Runtime pin

`Bundle.wxs` pins the .NET 10 Desktop Runtime patch:

- `ExePackagePayload/@Version` – runtime version (`10.0.14`)
- `Size` – remote file size in bytes
- `Hash` – **SHA-512** of the remote file (WiX `RemotePayload` uses SHA-512)
- `DownloadUrl` – Microsoft download URL
- `netfx:DotNetCoreSearch/@MajorVersion` – `10`

The matching SHA-256 is recorded in `docs/migration/installer-notes.md`. When
bumping the runtime, update all five values together and rebuild both projects.

## Verifying without installing

```powershell
# MSI contents (app + Avrdude, no PDBs); MSI lives under a culture subfolder
msiexec /a src/App/Installer/bin/Release/en-us/Fcc3Controller.msi /qn TARGETDIR=%TEMP%\fcc3-msi

# Burn bundle: downloads and hash-checks the runtime, extracts the MSI
src/App/Installer/bin/Release/Fcc3ControllerSetup.exe /layout %TEMP%\fcc3-layout /quiet
```

A full clean-VM check (no .NET 10 Desktop Runtime installed) is part of the
migration's V-1 verification.
