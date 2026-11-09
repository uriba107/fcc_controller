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
| MO1 | phase2 | Merge S‑A → target branch; app builds alone | S‑A DONE | integration branch | DONE | me | 2026-11-09T20:18Z | 2026-11-09T21:08Z | Merge commit `810cd9d`. `dotnet build FCC_configurator.csproj -c Release`: **0 errors, 0 warnings**. Deviation: dropped the redundant `System.Configuration.ConfigurationManager` PackageReference (it ships in the .NET 10 Windows Desktop shared framework; NU1510). Runtime smoke test still blocked by S‑B's not-yet-migrated GithubUpdater (expected; resolved at MO2). |
| MO2 | phase2 | Merge S‑B + S‑C → target; sln (sans vdproj) builds | MO1, S‑B, S‑C | integration branch | DONE | me | 2026-11-09T21:08Z | 2026-11-09T21:12Z | Merge commits `7e11e66` (S‑B) + `c20f7e1` (S‑C). Full net10 build of `FCC_configurator.csproj` (transitively builds all 3 SDK projects): **0 errors, 1 warning** (pre-existing `CS0168` `HexUpdater.cs:119`). Runtime smoke test: app starts, window `FCC Configurator - 1.0.2.0`, responsive for 8 s, no crash, no .NET Event Log errors. Note: the `.sln` still lists the `.vdproj`, so the build was done on the app project rather than the sln. |
| MO3 | phase2 | Merge S‑D → target; StickTracker renders | MO2, S‑D | integration branch | DONE | me | 2026-11-09T21:12Z | 2026-11-09T21:15Z | Merge commit `3d6870b`. Integrated build: 0 errors, 1 warning (pre-existing `CS0168`). Re-ran S‑D's chart harness against the integrated build: ChartAreaStick axes/minor-grid Dot, SeriesCurrent=Point, SeriesTrace=Spline, 150-pt stream rendered to PNG (visually confirmed: dotted minor grid, spline trace, point marker). |
| MO4 | phase2 | vdproj edits + MSI hand-build in VS | MO2, S‑G1 | `WindowsInstaller/WindowsInstaller.vdproj` | BLOCKED | me | 2026-11-09T21:22Z | | Needs the full Visual Studio IDE with the Installer Projects extension and a .NET 10 Desktop Runtime bootstrapper package. This machine only has VS 18 **BuildTools** (no `devenv`, no `BootstrapperPackages`, no `net10desktopruntime_x64`), so the MSI cannot be built or validated here. Follow `docs/migration/installer-notes.md` §5 (MO4 checklist) in VS 2026. Edits required: add `BootstrapperCfg` `.NET Desktop Runtime 10.0.14 (x64)`, switch ProjectOutput `Built`→`PublishItems` (`obj\Release\net10.0-windows\`), delete the 4 stale `IsDependency` entries, replace the .NET Framework 4.7.2 launch condition with `IsNETCore`. Avrdude is only packaged via Publish Items. |
| MO5 | phase2 | app.yml finalized to match real pipeline | MO3, MO4 | `.github/workflows/app.yml` | TODO | | | | |
| MO6 | phase2 | Update repo AGENTS.md + README building/release sections | MO3 | repo `AGENTS.md`, `README.md` | DONE | me | 2026-11-09T21:22Z | 2026-11-09T21:24Z | `AGENTS.md`: layout/CI/build sections now describe SDK-style `net10.0-windows`, `dotnet build`, framework-dependent output, PackageReferences, and the MO4/MO5 pending installer work; working-tree caution updated. `README.md`: added a "Building the configurator" section (build/run/release). |
| V‑1 | verify | Behavioral contract checks (run S‑F sheet) | MO3, MO4 | `docs/migration/checklist.md` (fill results col) | TODO | | | | |
| V‑2 | verify | Updater live-check vs real `uriba107/fcc_controller` release | MO2 | — | TODO | | | | |
| V‑3 | verify | Settings migration: user.config path, no crash | MO1 | — | TODO | | | | |
| V‑4 | verify | CI green: PR build + release-dispatch tag/assets | MO5 | `.github/workflows/app.yml` (fix-forward) | TODO | | | | |
| V‑5 | verify | Diff purity audit: per-stream diffs vs file ownership | MO3 | — | TODO | | | | |
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
