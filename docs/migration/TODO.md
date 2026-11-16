# Migration TODO — shared coordination board

Single source of truth for `docs/migration/net10-upgrade-plan.md`. Every agent (human or subagent)
must read this file **before starting work** and update it **after every status change**.
The plan document describes *how*; this board tracks *where we are*.

## Protocol (read this first)

1. **Claim before you work.** Set your task's row to `WIP`, fill `Owner`, `Started` (UTC
   timestamp, e.g. `2026-11-09T14:22Z`). No other agent may touch that task's files — file
   ownership per the plan is exclusive, but claim status makes it visible.
2. **Finish honestly.** On completion set `Status = DONE`, fill `Finished`, and put the
   merge-commit hash or worktree branch name in `Output/Notes`.
3. **Blocking?** Set `Status = BLOCKED` and describe exactly what you need in `Output/Notes`
   (name the blocking task ID and the concrete ask, e.g. "need S‑A csproj committed to
   branch X so PackageReference resolves").
4. **Handoffs.** Never rely on conversation memory — write anything the *next* agent needs
   in the Handoff Notes section at the bottom. Check that section when you start.
5. **Stale work.** If a row is `WIP` with no `Finished` and you're told to take over:
   re-verify on the branch, note takeover in `Output/Notes`, keep a new `Owner`.
6. **One status change = one file edit.** Only update your own rows and Handoff Notes.
   Do not reorganize or reword other agents' entries.
7. This file is the only place agents sync. Do not invent side channels — after finishing,
   assume the next agent reads **only this file plus the plan**.

Status values: `TODO` / `WIP` / `BLOCKED` / `DONE` / `N.A.` (skipped, with reason in notes)

## Task board

| ID | Stream | Task | Depends on | Owns (files) | Status | Owner | Started | Finished | Output / Notes |
|----|--------|------|-----------|--------------|--------|-------|---------|----------|----------------|
| P0‑1 | preflight | Resolve HidSharp working-tree diff (commit or stash) | — | `src/App/*` working tree | DONE | me | 2026-11-09T20:02Z | 2026-11-09T20:05Z | Tracked tree was already clean (HidSharp 2.6.4/net48 work committed on `develop` as `b2c14ed`, `f40b214`). Removed stale `src/App/UpgradeLog*.htm` VS migration reports and gitignored them. Commit `3721fa6`. |
| P0‑2 | preflight | Baseline net48 build + smoke notes (append to plan Appendix) | — | `docs/migration/net10-upgrade-plan.md` (appendix) | DONE | me | 2026-11-09T20:05Z | 2026-11-09T20:12Z | net48 Release `/t:Rebuild`: 0 errors, 1 warning (`CS0168` `HexUpdater.cs:119`). Smoke: MainForm v1.0.2.0 launches and is responsive on Win11; other forms not exercised (no device). Needs `/p:FrameworkPathOverride` (no 4.8 targeting pack) and `/p:SignManifests=false` (no SignTool). Full notes in plan Appendix. |
| P0‑3 | preflight | Decide gates: Chart = Dv port; MSI = framework-dependent | — | plan §Decision Gates | DONE | me | 2026-11-09T20:12Z | 2026-11-09T20:12Z | Gates confirmed as written in plan §Decision Gates: chart port via `WinForms.DataVisualization`; framework-dependent MSI with .NET 10 Desktop Runtime chained prerequisite (self-contained single-file is fallback). |
| P0‑4 | preflight | Branch `migration/target-net10` + per-stream worktrees | P0‑1 | git/worktrees | DONE | me | 2026-11-09T20:12Z | 2026-11-09T20:18Z | `migration/target-net10` from `develop` (d1449b7). Worktrees under `%LOCALAPPDATA%\Temp\opencode\fcc-net10\`: `s-a`→`migration/s-a-config-infra`, `s-b`→`migration/s-b-github-updater`, `s-c`→`migration/s-c-hexhelper`, `s-e`→`migration/s-e-ci`, `s-f`→`migration/s-f-checklist`, `s-g1`→`migration/s-g1-installer-notes`. S‑D worktree created later from S‑A's branch. |
| S‑A | phase1 | FCC_configurator csproj → SDK-style net10 + deps | P0‑3, P0‑4 | `FCC_configurator.csproj`, `packages.config`, `App.config`, `Properties/Settings*` | DONE | subagent-s-a | 2026-11-09T20:20Z | 2026-11-09T20:36Z | Build green: 0 errors, 3 warnings (2× NU1510 for the required `System.Configuration.ConfigurationManager`, 1× CS0168 in HexHelper — not owned). Output `src\App\Fcc3_configurator\bin\Release\net10.0-windows\`. App.config verified read (temp default 7.5 was picked up + persisted to user.config). Smoke: window title `FCC Configurator - 1.0.2.0` appears, but process self-terminates ~1.6s later from old-style `GithubUpdater` (`System.Web.Extensions`/`JavaScriptSerializer`) — S‑B's file, not S‑A. Deviation: added `WinForms.DataVisualization` 1.10.2 (task required) and `NoWarn=CA1416` (false positive from `GenerateAssemblyInfo=false`). Commit: 52d837042a6aad7d33c8f46940d8531adddca6cf |
| S‑B | phase1 | GithubUpdater → System.Text.Json | P0‑3, P0‑4 | `GithubUpdater.csproj`, `GithubUpdater.cs` | DONE | subagent-s-b | 2026-11-09T20:25Z | 2026-11-09T20:29Z | Commit `184cef2` (branch `migration/s-b-github-updater`). `dotnet build -c Release`: **0 warnings, 0 errors** → `bin\Release\net10.0\GithubUpdater.dll`. Live smoke test vs real `uriba107/fcc_controller`: Update=True, firmware `1.0.6` from `fcc_controller_1.0.6.hex`, app `1.0.2.0` from `fcc_controller_1.0.2.0.msi`, Changelog=`html_url`, CurrentRelease=`180601`; both downloads succeeded. Asset-picking logic unchanged (`.hex`/`.msi` FirstOrDefault + `Version.TryParse` on `.Replace(...)`); null-safe JSON access added. |
| S‑C | phase1 | HexHelper → System.IO.Ports | P0‑3, P0‑4 | `HexHelper.csproj`, `HexUpdater.cs` | DONE | subagent-s-c | 2026-11-09T20:25Z | 2026-11-09T20:29Z | Code commit `07ead17` on `migration/s-c-hexhelper` (board update `docs/migration/TODO.md` follows). SDK-style `net10.0-windows` lib: `AssemblyName`/`RootNamespace` HexUploader, `GenerateAssemblyInfo=false` (keeps `Properties/AssemblyInfo.cs`), `System.IO.Ports` 10.0.14. `HexUpdater.cs` unchanged. `dotnet build -c Release` (also `-t:Rebuild`): **0 errors, 1 warning** (pre-existing `CS0168` `HexUpdater.cs:119`). Verified `bin\Release\net10.0-windows\Avrdude\` contains `avrdude.conf` (20623 B), `avrdude.exe` (407040 B), `libusb0.dll` (67680 B). Deviation from the task's literal snippet: `Content Update` matched nothing because the SDK default `None` glob already includes `Avrdude\*`; used `None Remove` + `Content Include` for the two binaries (still `Content`, no duplicates), `None Update` for the `.conf`. |
| S‑D | phase1 | Chart port (FormStickTracker via Dv NuGet) | S‑A | `FormStickTracker.cs`, `FormStickTracker.Designer.cs` | DONE | subagent-s-d | 2026-11-09T20:59Z | 2026-11-09T21:02Z | Build green (0 errors; 3 pre-existing warnings unrelated to chart). Harness on net10.0-windows confirms ChartAreaStick AxisX/Y enabled, AxisX2/Y2 disabled, minor grid Dot; SeriesCurrent=Point (1 pt), SeriesTrace=Spline; 150-pt stream rendered to PNG. No code change needed. Commit: ac0c94f17f0d9920825a25eb6c397866615c33e7 |
| S‑E | phase1 | CI workflows draft (setup-dotnet 10.0.x) | P0‑3, P0‑4 | `.github/workflows/app.yml` | DONE | subagent-s-e | 2026-11-09T20:27Z | 2026-11-09T20:31Z | Commit 9badb36. YAML sanity: PyYAML safe_load OK, no tabs. Output path confirmed with a throwaway net10.0-windows SDK build (bin\Release\net10.0-windows\). Draft: vdproj/MSI steps kept with continue-on-error, marked PENDING MO4/MO5; release builds Release only. setup-dotnet v6.1.0 is latest (2026-11-03); used v5 per task. |
| S‑F | phase1 | Regression checklist doc (contracts from repo AGENTS.md) | P0‑3, P0‑4 | `docs/migration/checklist.md` | DONE | subagent-s-f | 2026-11-09T20:43Z | 2026-11-09T20:48Z | Commit `da91e75` on `migration/s-f-checklist`. Added `docs/migration/checklist.md`: 15-row executable acceptance table (device connect, HID IN decode with known reports, HID OUT Center/Reboot, options bits 0x01-0x80 vs `fcc3.h`, config mode, fw-update/avrdude flow, GithubUpdater, single-instance mutex, settings/user.config, fw version). Result column left blank for tester. |
| S‑G1 | phase1 | Installer research ← notes only, no edits | P0‑3, P0‑4 | `docs/migration/installer-notes.md` | DONE | subagent-s-g1 | 2026-11-09T20:49Z | 2026-11-09T20:55Z | Commit `1b80652`. Key findings: .NET 10 Desktop Runtime bootstrapper = `Microsoft.NetCore.DesktopRuntime.10.0.x64` (folder `net10desktopruntime_x64`, 10.0.14 x64 URL/SHA in notes); vdproj must switch `Built`→`PublishItems` (canonical name), delete 4 stale detected deps, replace .NET Framework 4.7.2 launch condition with `IsNETCore`; **current vdproj dropped the old Avrdude file entries — Avrdude is NOT packaged until Publish Items is used**; CI runner (VS 2022 17.14, ext 3.0.0, ClickOnce 17.14.36510.44, no .NET 10 runtime component) does not list .NET 10 in Prerequisites — build on VS 2026 or add the package manually. |
| S‑G2 | phase1B | WiX v5 MSI + Burn bundle, vdproj removal, updater asset preference | MO3 | `src/App/Installer/**`, `src/App/Fcc3_configurator.sln`, `src/App/WindowsInstaller/**`, `src/App/GithubUpdater/GithubUpdater.cs` | DONE | me | 2026-11-14T08:30Z | 2026-11-14T10:15Z | New `src/App/Installer/` (SDK-style `Installer.wixproj` + `Product.wxs`, `Bundle.wixproj` + `Bundle.wxs`, `Directory.Build.props` for per-project `obj/`, `.gitignore`, `README.md`). MSI: framework-dependent publish harvest, `Avrdude\` included, PDBs excluded, x64, ProductVersion `1.0.2`, old vdproj `UpgradeCode` reused. Bundle: `DotNetCoreSearch` + remote `ExePackage` for .NET Desktop Runtime 10.0.14 x64 with SHA‑512. vdproj removed from the solution and `git rm`'d. Updater prefers `fcc_controller_<v>.exe` (Burn), falls back to `.msi`, keeps `FirstOrDefault`/version parsing. Also fixed a latent NRE: `Asset` is a struct, so `FirstOrDefault()` assigned to `Asset?` yielded a non-null default and skipped the fallback (null `name`); both firmware/app selections now project `(Asset?)a` first. |
| MO1 | phase2 | Merge S‑A → target branch; app builds alone | S‑A DONE | integration branch | DONE | me | 2026-11-09T20:18Z | 2026-11-09T21:08Z | Merge commit `810cd9d`. `dotnet build FCC_configurator.csproj -c Release`: **0 errors, 0 warnings**. Deviation: dropped the redundant `System.Configuration.ConfigurationManager` PackageReference (it ships in the .NET 10 Windows Desktop shared framework; NU1510). Runtime smoke test still blocked by S‑B's not-yet-migrated GithubUpdater (expected; resolved at MO2). |
| MO2 | phase2 | Merge S‑B + S‑C → target; sln (sans vdproj) builds | MO1, S‑B, S‑C | integration branch | DONE | me | 2026-11-09T21:08Z | 2026-11-09T21:12Z | Merge commits `7e11e66` (S‑B) + `c20f7e1` (S‑C). Full net10 build of `FCC_configurator.csproj` (transitively builds all 3 SDK projects): **0 errors, 1 warning** (pre-existing `CS0168` `HexUpdater.cs:119`). Runtime smoke test: app starts, window `FCC Configurator - 1.0.2.0`, responsive for 8 s, no crash, no .NET Event Log errors. Note: the `.sln` still lists the `.vdproj`, so the build was done on the app project rather than the sln. |
| MO3 | phase2 | Merge S‑D → target; StickTracker renders | MO2, S‑D | integration branch | DONE | me | 2026-11-09T21:12Z | 2026-11-09T21:15Z | Merge commit `3d6870b`. Integrated build: 0 errors, 1 warning (pre-existing `CS0168`). Re-ran S‑D's chart harness against the integrated build: ChartAreaStick axes/minor-grid Dot, SeriesCurrent=Point, SeriesTrace=Spline, 150-pt stream rendered to PNG (visually confirmed: dotted minor grid, spline trace, point marker). |
| MO4 | phase2 | WiX installer: MSI + Burn bundle (VS-independent) | MO2, S‑G2 | `src/App/Installer/**`, `GithubUpdater/GithubUpdater.cs`, `Fcc3_configurator.sln` | DONE | me | 2026-11-14T08:30Z | 2026-11-14T10:30Z | Re-scoped 2026-11-09 from the VS-only vdproj path to WiX Toolset v5 (plan Decision Gate 2). Builds: `dotnet build src/App/Installer/Installer.wixproj -c Release` → `bin/Release/en-us/Fcc3Controller.msi`; `dotnet build src/App/Installer/Bundle.wixproj -c Release` → `bin/Release/Fcc3ControllerSetup.exe`; both **0 warnings, 0 errors**. MSI checked with `msiexec /a` (app + deps + `Avrdude\avrdude.exe\|conf\|libusb0.dll`, no PDBs) and `wix msi validate` (pass). Bundle `Fcc3ControllerSetup.exe /layout … /quiet` exit 0 and fetched runtime 10.0.14 (size `60035296` + SHA‑512 verified). Updater re-checked live: `.msi` fallback → `1.0.2.0`; synthetic `.exe` → `9.9.9`; missing-asset path safe. Deviation: clean-VM install/uninstall (runtime chain, Avrdude on disk) still needs a VM → tracked under V‑1, not claimed here. |
| MO5 | phase2 | app.yml finalized to match real pipeline | MO3, MO4 | `.github/workflows/app.yml` | DONE | me | 2026-11-14T09:30Z | 2026-11-14T10:45Z | Release job now publishes the app (`dotnet publish -c Release`), builds `Bundle.wixproj` (which MSBuild-builds the MSI via `ProjectReference`), and stages `fcc_controller_<version>.exe` + `fcc_controller_<version>.msi` with `-Recurse` (MSI lands in a culture subfolder). Removed all `devenv`/`DisableOutOfProcBuild`/vdproj steps. PR job still build-only; firmware asset carry-forward untouched. |
| MO6 | phase2 | Update repo AGENTS.md + README building/release sections | MO3 | repo `AGENTS.md`, `README.md` | DONE | me | 2026-11-09T21:22Z | 2026-11-09T21:24Z | `AGENTS.md`: layout/CI/build sections now describe SDK-style `net10.0-windows`, `dotnet build`, framework-dependent output, PackageReferences, and the MO4/MO5 pending installer work; working-tree caution updated. `README.md`: added a "Building the configurator" section (build/run/release). |
| V‑1 | verify | Behavioral contract checks (run S‑F sheet) | MO3, MO4 | `docs/migration/checklist.md` (fill results col) | BLOCKED | me | 2026-11-09T21:32Z | | Needs the physical FCC3 USB device (VID `0x1029` / PID `0xF16C`) and the MO4 installer; none is available in this environment. `docs/migration/checklist.md` is ready for the tester. Once MO4 lands, also run the installer checks: clean-VM install via the Burn bundle (runtime chained), Avrdude present, uninstall clean. |
| V‑2 | verify | Updater live-check vs real `uriba107/fcc_controller` release | MO2 | — | DONE | subagent-v2 | 2026-11-09T21:26Z | 2026-11-14T10:35Z | Harness against the integrated `GithubUpdater.dll`: `Update()=True`, firmware `1.0.6` (`fcc_controller_1.0.6.hex`), app `1.0.2.0` (`fcc_controller_1.0.2.0.msi`), `CurrentRelease=180601`, `Changelog` = release `html_url`; matches the real latest release. Null-path probe: `Update()=False`, versions `0.0.0`/`0.0.0.0`, file names null, checks false. **Re-run after S‑G2** with `.exe` preference: live release (no `.exe`) still resolves `.msi` `1.0.2.0`; synthetic release with both picks `fcc_controller_9.9.9.exe`; `.msi`-only synthetic still resolves `1.0.2.0`. |
| V‑3 | verify | Settings migration: user.config path, no crash | MO1 | — | DONE | me | 2026-11-09T21:30Z | 2026-11-09T21:32Z | net10 app writes `%LOCALAPPDATA%\Fcc3_configurator\FCC_configurator_Url_<hash>\1.0.2.0\user.config` (new profile location; old net48 `FCC_configurator.exe_Url_*` profile left untouched → one-time defaults reset, expected). File contains `gUserDefinedForce=3.0`, `notifyApp=True`, `notifyFirmware=True`. No crash (MO2 smoke test). |
| V‑4 | verify | CI green: PR build + release-dispatch tag/assets | MO5 | `.github/workflows/app.yml` (fix-forward) | BLOCKED | me | 2026-11-09T21:32Z | | MO5 is now implemented locally (WiX build + `.exe`/`.msi` staging), but no CI run is possible from this environment. Needs a push/PR and a `release=true` dispatch; expect a `firmware-<version>` artifact on push and `app-v<version>` + `.exe`/`.msi`/carried-forward `.hex` on release. |
| V‑5 | verify | Diff purity audit: per-stream diffs vs file ownership | MO3 | — | DONE | subagent-v5 | 2026-11-09T21:26Z | 2026-11-09T21:31Z | All 7 stream diffs confined to their declared files (+ `docs/migration/TODO.md`). No `src/Controller/**`, `src/LUFA/**`, `.vdproj`, `firmware.yml`, or `FormStickTracker*` changes. Integrated diff = union of streams + planned MO6 docs. GithubUpdater asset-picking logic preserved exactly (first `.hex`/`.msi`, `.Replace(...)` version parse, `FirstOrDefault` + null guards). |
| R‑1 | release | Ride next `app-v` release dispatch | all verify DONE | — | TODO | | | | |
| R‑2 | release | Reconciliation actuals vs estimates (plan appendix) | R‑1 | plan appendix | TODO | | | | |

## Merge ladder order (phase 2)

`MO1 → MO2 → MO3 → MO4 → MO5 → MO6` — each session itself performs the merges, so this
ladder is what "DONE" in phase-1 rows must trigger: phase-1 DONE means "in my worktree,
buildable".

## Handoff Notes (append, newest at bottom)

---

### 2026-11-09T20:12Z — orchestrator (me)

- Environment setup done during P0:
  - .NET SDK `10.0.403` installed user-locally at `C:\Users\me\.dotnet\dotnet.exe` (no admin;
    not on `PATH`). Use the full path for all `dotnet` commands.
  - net48 baseline needs `/p:FrameworkPathOverride=<ref-assemblies>\build\.NETFramework\v4.8`
    (package `Microsoft.NETFramework.ReferenceAssemblies.net48` 1.0.3 extracted under
    `%LOCALAPPDATA%\Temp\opencode\net48-ref`) and `/p:SignManifests=false /p:SignAssembly=false`.
- Branching model: integration branch `migration/target-net10` (from `develop`); one branch +
  worktree per stream, merged in the MO ladder. Worktrees live under
  `%LOCALAPPDATA%\Temp\opencode\fcc-net10\<stream>`.
- Next agent: start Phase 1 streams S‑A/B/C/E/F/G1 (S‑D after S‑A lands), then run the MO
  merge ladder.

### 2026-11-09T20:18Z — orchestrator (me)

- Phase 1 launched: subagents for S‑A, S‑B, S‑C, S‑E, S‑F, S‑G1 in parallel, each in its own
  worktree/branch listed in P0‑4. S‑D will be launched from S‑A's branch once S‑A reports DONE.
- Note for S‑A: the plan says S‑D re-adds the chart package, but S‑D does not own the csproj, so
  S‑A must add the `WinForms.DataVisualization` PackageReference for the app to build. S‑D then
  validates the chart code/designer only.

### 2026-11-09T21:08Z — orchestrator (me)

- MO1 done: S‑A merged as `810cd9d`; app project builds 0 warnings / 0 errors.
- Deviation from the plan: removed the `System.Configuration.ConfigurationManager`
  PackageReference. It is part of the .NET 10 Windows Desktop shared framework
  (`...\shared\Microsoft.WindowsDesktop.App\10.0.3\System.Configuration.ConfigurationManager.dll`),
  so referencing it explicitly only produced NU1510. `Settings.Default` still works.
- Next: MO2 (merge S‑B + S‑C, full build + runtime smoke test), then MO3 (S‑D).

### 2026-11-09T21:22Z — orchestrator (me)

- All Phase 1 streams merged: S‑A `810cd9d`, S‑B `7e11e66`, S‑C `c20f7e1`, S‑D `3d6870b`,
  S‑E `82ad3f4`, S‑F `96537a8`, S‑G1 `3755f76`.
- MO1/MO2/MO3 done; integrated net10 build is green (0 errors, 1 pre-existing warning).
- **MO4 is BLOCKED** on the full VS IDE + Installer Projects + .NET 10 bootstrapper. This
  machine has VS 18 BuildTools only (no `devenv`, no `BootstrapperPackages`). The user must
  perform MO4 in VS 2026 using `docs/migration/installer-notes.md` §5. Do not hand-edit the
  vdproj without the ability to build the MSI.
- MO5 (app.yml finalization) is partially unblocked: the build job is already correct; only the
  vdproj/MSI steps depend on MO4. MO6 (docs) is unblocked.

### 2026-11-09T21:35Z — orchestrator (me)

- MO6 done; `AGENTS.md` + `README.MD` now describe the net10 build/run/release flow. Plan
  checklist synced.
- Phase 3: **V‑2, V‑3, V‑5 DONE**; **V‑1 BLOCKED** (needs the physical FCC3 device); **V‑4
  BLOCKED** (needs MO5/MO4 + a CI run).
- Remaining work is gated on MO4 (VS-only installer):
  1. MO4 in VS 2026 following `docs/migration/installer-notes.md` §5.
  2. MO5: finalize `.github/workflows/app.yml` vdproj/MSI steps after MO4.
  3. V‑1: run `docs/migration/checklist.md` with the physical device.
  4. V‑4: push `migration/target-net10` / open a PR to validate CI.
- Integrated branch `migration/target-net10` builds clean (0 errors, 1 pre-existing warning) and
  the app runs. No firmware, LUFA, or `.vdproj` files were touched.

### 2026-11-09T21:49Z — orchestrator (me)

- Re-scoped the installer to remove the Visual Studio dependency. The plan now uses WiX
  Toolset v5 (MSI + Burn bundle) instead of the `.vdproj` + Installer Projects path.
- Why VS was required: (1) `.vdproj` is a VS-only project type built by `devenv` with the
  Installer Projects extension, not by MSBuild/BuildTools; (2) the .NET 10 Desktop Runtime
  bootstrapper package ships only with VS 2026, not VS 2022 or the CI runner. WiX removes
  both: it is a .NET tool/MSBuild SDK, and Burn chains the runtime itself.
- MO4 is now TODO (was BLOCKED on VS). New stream S‑G2 owns the WiX project, removes the
  vdproj, and updates `GithubUpdater` to prefer the `.exe` bundle (`.msi` fallback).
- Plan updated: Decision Gate 2, new "Why Visual Studio was required" section, Phase 1B,
  MO4/MO5, V‑1/V‑2/V‑4/V‑5, master checklist, DAG, parallelism, toolchain appendix.
- Next: launch S‑G2 in a worktree off `migration/target-net10`, then MO4 → MO5 → V‑1/V‑4.

### 2026-11-14T10:50Z — orchestrator (me)

- S‑G2 / MO4 / MO5 implemented and locally validated (details in the board rows above).
  Everything is still uncommitted on `migration/target-net10` at the time of writing.
- Installer outputs: `src/App/Installer/bin/Release/en-us/Fcc3Controller.msi` and
  `src/App/Installer/bin/Release/Fcc3ControllerSetup.exe`. The MSI's file table was checked by
  admin-install (`msiexec /a`): app + deps + `Avrdude\` present, PDBs absent. The bundle was
  checked with `/layout` (it fetched runtime 10.0.14 and matched the pinned size/SHA‑512).
  A real clean-VM install/uninstall still belongs to V‑1 (no VM here).
- Updater: live release still resolves the `.msi` fallback (`1.0.2.0`) because no release has
  a Burn `.exe` yet; a synthetic release with both assets picks the `.exe`; `.msi`-only still
  works. Also fixed a latent `Asset`-struct/`Asset?` NRE in `GetAssets()` (would have crashed
  the live path after the `.exe` preference was added).
- Not done here: V‑4 CI run, V‑1 device/VM checks, R‑1/R‑2. Signing is still out of scope
  (unsigned MSI/bundle, same as the previous vdproj release).
- Next: commit the implementation + docs, run the review pass, then push/PR for V‑4 and run
  V‑1 with the device + a clean VM.

### 2026-11-14T11:30Z — orchestrator (me) — review pass

- Ran `ocr review --from develop --to migration/target-net10` (commit `cac0b61`); 16 findings.
  Verified each against the code before changing anything.
- Fixed in the first pass: `GetAssets()` clears `Assets` so repeated `Update()` calls cannot
  resolve stale assets or grow the list; downloads also guard the asset `name`; portable
  staging copies recursively (`runtimes\`/`Avrdude\` were copied empty); `dist\bundle` is
  removed after zipping. The high-severity "MSI ships without Avrdude" was a **false
  positive** — `HexUploader` declares `Avrdude\*` as Content, so `dotnet publish` emits it
  (re-verified with a clean publish and the MSI file table).
- Fixed in the second pass, after re-checking the rejected items:
  - `Update()` runs on a worker thread (`RunUpdateManager`), while the UI timer reads
    `FirmwareVersion`/`AppVersion`/`Check*`; all shared state now goes through `stateLock`.
    The lock never wraps network I/O, so the UI cannot stall on HTTP.
  - `DownloadFirmware`/`DownloadApp` now return `false` on HTTP/file failures instead of
    throwing into the UI click handlers (partial files may remain, as before); `GetData` and
    both downloads log failures via `Debug.WriteLine` instead of swallowing them silently.
  - `app.yml`: `timeout-minutes` 30 (build) / 45 (release) and an empty-version guard before
    staging the installer assets.
- Rejected after re-verification: blocking `GetAwaiter().GetResult()` reads (the sync
  `HttpClient.Send` already blocks; async is a separate redesign), nullable `CurrentRelease`
  (not consumed by the app and `Nullable` is disabled), and NuGet caching (`cache: true`
  needs a `packages.lock.json` or explicit dependency path; none exists here).

### 2026-11-14T12:30Z — orchestrator (me) — second review pass

- Re-ran `ocr` on `develop..migration/target-net10` (15 findings) and fixed the valid ones:
  - asset matching now uses `Path.GetExtension(...)` with `OrdinalIgnoreCase` instead of
    `Contains(".hex"/".exe"/".msi")`, so `*.hex.sig` / `*.exe.txt` cannot be selected;
  - download targets must be plain file names (`Path.GetFileName(name) == name`) and are
    written with `Path.Combine`, closing a path-traversal route from remote asset names;
  - `GetData()` returns success/failure and `Update()` returns `false` when a refresh failed
    (last-known data is kept for the UI); logging uses `Trace` so it survives Release builds;
  - workflow: the zip step has the same empty-version guard, and the release fails if
    `Avrdude\avrdude.exe` is missing from the publish output.
- Rejected: sync-over-async HTTP (pre-existing sync `HttpClient.Send`), NuGet caching (no
  `packages.lock.json` to key on), the mixed 3/4-part version in the portable zip name (the
  updater only parses `.exe`/`.msi`/`.hex`), and removing `continue-on-error` from the firmware
  carry-forward (a first release legitimately has no previous `.hex`; the updater tolerates it).

### 2026-11-14T13:45Z — orchestrator (me) — third review pass

- Re-ran `ocr` (9 findings) and fixed the valid ones:
  - downloads stream `HttpContent.ReadAsStream()` straight to `File.Create` instead of buffering
    the whole payload (`ReadAsByteArrayAsync`) in memory, and `GetData` reads the JSON through a
    `StreamReader` — no more `GetAwaiter().GetResult()`;
  - download guards use `string.IsNullOrEmpty` (null-safe) and the plain-file-name check moved
    inside the try, so a null `TargetPath`/invalid name returns `false` instead of throwing;
  - `CheckFirmware`/`CheckApp` use `Version.TryParse`, so a malformed device/app version means
    "no update" instead of an exception on the UI timer;
  - CI now verifies the publish directory, `FCC_configurator.exe` and `Avrdude\avrdude.exe`,
    and enables `setup-dotnet` NuGet caching keyed on `src/App/**/*.csproj`.
- Verified the download path for real: firmware asset downloaded (25,729 bytes) into a temp
  dir, and a synthetic `..\evil.hex` asset name was rejected with no file written outside the
  target directory.
- Rejected: requiring the repo prefix on the `.msi` fallback (kept permissive for older
  releases; an unparseable version only disables the "update available" hint).
- Remaining low-severity items (native `runtimes\` bloat in the MSI, first-asset ordering when
  a release has multiple `.msi` assets) are accepted as-is.

### 2026-11-14T15:20Z — orchestrator (me) — AVRDUDE 8.3, trimmed conf, backend question

- Bundled AVRDUDE 6.0.1 (2015) → **8.3** (official MSVC x64, statically linked: no VC++ redist,
  no `libusb0.dll`). `libusb0.dll` deleted, `HexHelper.csproj` content list updated.
- `avrdude.conf` trimmed from upstream 24,865 lines / 931 KB to 262 lines / 9,431 B: only the
  `avr109`/`avr911` programmer plus the `.classic-nocal → .classic → m16u4 → m32u4` part chain.
  Verified with AVRDUDE's `dryrun` programmer: identical memory table to upstream (flash 32768/128,
  eeprom 1024/4, sram 2560, signature `1E 95 87`) and `Fcc3.hex` writes + verifies.
- Checked whether the configurator ever used `uriba107/32u4_hexuploader` /
  `uriba107/ArduinoSketchUploader` as the upload backend: **it never did**. The only trace is a
  commented-out `ArduinoSketchUploader` snippet in the initial commit `1ee8291` (2017-03-25),
  removed in `9d13401` (2017-04-01); no branch, worktree, csproj or package reference has it.
- Deferred (user decision: keep avrdude for now): swapping the `Process.Start` avrdude path for
  the managed AVR109 implementation. Feasibility already proven in a scratch build — the fork's
  `master` (upstream + `TriggerBootloader` flag, `10cea83a`) builds on `net10.0-windows` with
  `RJCP.SerialPortStream 3.0.5` + `IntelHexFormatReader 2.2.3` after replacing 4
  `SerialPortStream.GetPortNames()` calls with `SerialPort.GetPortNames()`. It would drop
  `Avrdude\` (≈9.7 MB) and the external process entirely, but needs bench validation on a real
  FCC3 bootloader before it can ship.
- CI: the `build` job now also runs `dotnet publish`, verifies the `Avrdude\` payload, builds
  `Bundle.wixproj` (which builds the MSI) and asserts both installer outputs, so every push/PR
  exercises the WiX pipeline instead of only a release dispatch. The `release` job keeps its own
  copies so a release does not depend on the build job's artifacts.
