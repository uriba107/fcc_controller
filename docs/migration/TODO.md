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
| P0‑4 | preflight | Branch `migration/target-net10` + per-stream worktrees | P0‑1 | git/worktrees | WIP | me | 2026-11-09T20:12Z | | Branch `migration/target-net10` created from `develop`; per-stream worktrees/branches being created. |
| S‑A | phase1 | FCC_configurator csproj → SDK-style net10 + deps | P0‑3, P0‑4 | `FCC_configurator.csproj`, `packages.config`, `App.config`, `Properties/Settings*` | TODO | | | | |
| S‑B | phase1 | GithubUpdater → System.Text.Json | P0‑3, P0‑4 | `GithubUpdater.csproj`, `GithubUpdater.cs` | DONE | subagent-s-b | 2026-11-09T20:25Z | 2026-11-09T20:29Z | Commit `184cef2` (branch `migration/s-b-github-updater`). `dotnet build -c Release`: **0 warnings, 0 errors** → `bin\Release\net10.0\GithubUpdater.dll`. Live smoke test vs real `uriba107/fcc_controller`: Update=True, firmware `1.0.6` from `fcc_controller_1.0.6.hex`, app `1.0.2.0` from `fcc_controller_1.0.2.0.msi`, Changelog=`html_url`, CurrentRelease=`180601`; both downloads succeeded. Asset-picking logic unchanged (`.hex`/`.msi` FirstOrDefault + `Version.TryParse` on `.Replace(...)`); null-safe JSON access added. |
| S‑C | phase1 | HexHelper → System.IO.Ports | P0‑3, P0‑4 | `HexHelper.csproj`, `HexUpdater.cs` | TODO | | | | |
| S‑D | phase1 | Chart port (FormStickTracker via Dv NuGet) | S‑A | `FormStickTracker.cs`, `FormStickTracker.Designer.cs` | TODO | | | | |
| S‑E | phase1 | CI workflows draft (setup-dotnet 10.0.x) | P0‑3, P0‑4 | `.github/workflows/app.yml` | TODO | | | | |
| S‑F | phase1 | Regression checklist doc (contracts from repo AGENTS.md) | P0‑3, P0‑4 | `docs/migration/checklist.md` | TODO | | | | |
| S‑G1 | phase1 | Installer research ← notes only, no edits | P0‑3, P0‑4 | `docs/migration/installer-notes.md` | TODO | | | | |
| MO1 | phase2 | Merge S‑A → target branch; app builds alone | S‑A DONE | integration branch | TODO | | | | |
| MO2 | phase2 | Merge S‑B + S‑C → target; sln (sans vdproj) builds | MO1, S‑B, S‑C | integration branch | TODO | | | | |
| MO3 | phase2 | Merge S‑D → target; StickTracker renders | MO2, S‑D | integration branch | TODO | | | | |
| MO4 | phase2 | vdproj edits + MSI hand-build in VS | MO2, S‑G1 | `WindowsInstaller/WindowsInstaller.vdproj` | TODO | | | | |
| MO5 | phase2 | app.yml finalized to match real pipeline | MO3, MO4 | `.github/workflows/app.yml` | TODO | | | | |
| MO6 | phase2 | Update repo AGENTS.md + README building/release sections | MO3 | repo `AGENTS.md`, `README.md` | TODO | | | | |
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
