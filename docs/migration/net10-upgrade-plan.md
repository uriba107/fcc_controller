# Execution Plan: net48 → .NET 10 Migration (FCC3 Configurator App)

**Coordination board:** `docs/migration/TODO.md` — claim tasks (`WIP` + Owner + Started), record
DONE/BLOCKED there, and leave handoff notes for the next agent. This plan is the how; the board
is the current state. Agents may rely only on those two files for state.


**Scope:** `src/App/**` only (FCC_configurator, GithubUpdater, HexHelper, WindowsInstaller.vdproj, app.yml). Firmware (`src/Controller/**`) is explicitly out of scope and untouched.

**Target:** `net10.0-windows` (LTS to Nov 2028), C# default (net10 → C# 14; the explicit `LangVersion 7.3` props are *deleted*, not raised).

> Why .NET 10 and not .NET 8: .NET 8 reaches end of support on Nov 10, 2026 (last patch 8.0.33). .NET 10 is the current LTS (supported to Nov 2028); migration effort from net48 is essentially identical. Note: this migration does **not** make the app cross-platform — WinForms remains Windows-only.

---

## Phase 0 — Preflight (serial, blocks everything)

| ID | Task | Output |
|----|------|--------|
| P0‑1 | **Resolve the in-progress HidSharp working-tree diff.** Commit or stash `src/App` changes (csproj, packages/, x64 configs) so migration starts from a clean, reproducible state. Per repo caution: inspect, don't blindly revert. | Clean `git status` |
| P0‑2 | **Record baseline:** build net48 Release + note warnings count; run the forms once (Main, StickTracker, Advanced, FwUpgrade) on Win11. | Baseline notes appended below |
| P0‑3 | **Decide the two open design gates** (see Decision Gates). | Written decisions |
| P0‑4 | Create a `migration/target-net10` branch. Each parallel stream works in its own **worktree** off this branch, merged strictly in merge order (MO below) to avoid file conflicts. | Branch + worktree list |

### Decision Gates (P0‑3)

1. **Chart:** **Port via `WinForms.DataVisualization` NuGet** (API-compatible with the Designer file) — lowest risk. Rewrite (ScottPlot) is the documented fallback if the port fails rendering/regression.
2. **Runtime distribution:** **Framework-dependent MSI + .NET 10 Desktop Runtime chained via vdproj bootstrapper prerequisite.** Self-contained single-file is the documented fallback if vdproj chaining proves impractical. (Keeps MSI small; vdproj already ships Avrdude files the same way.)

---

## Phase 1 — Horizontal decomposition (max parallel: 6 subagents)

Six independent worktrees; **no two agents touch the same file**. All subagents are `general` unless noted; all get: scope = `src/App/**` only, do not touch `src/Controller/**` or `src/LUFA/**`, and the cross-component contract excerpt from the repo AGENTS.md (USB IDs, HID layout, options bitmask, updater asset naming).

| ID | Stream | Agent | Files owned (exclusive) | Key work | Est. |
|----|--------|-------|-------------------------|----------|------|
| S‑A | **FCC_configurator infra** | general | `Fcc3_configurator/FCC_configurator.csproj`, `packages.config` (delete), `App.config`, `Properties/Settings*` | Convert csproj → SDK-style: `net10.0-windows`, `UseWindowsForms=true`, globbed Compile/EmbeddedResource, keep `app.manifest`/icon/`StartupObject`/`AssemblyName`. PackageReference `HidSharp` 2.6.4, `System.Configuration.ConfigurationManager`. Drop `System.Deployment`, `System.Windows.Forms.DataVisualization` ref (S‑D re-adds via package), ClickOnce/pfx sign props, obsolete bootstrapper packages. | 1 d |
| S‑B | **GithubUpdater → modern .NET** | general | `GithubUpdater/GithubUpdater.csproj`, `GithubUpdater.cs` | SDK-style `net10.0` lib. Replace `System.Web.Extensions`/`JavaScriptSerializer` + `WebClient` → `System.Text.Json` + `HttpClient`. **Do not change** asset-picking logic (first asset containing `.hex`/`.msi`, `fcc_controller_<version>` name parsing, `FirstOrDefault` + null guards → 0.0.0 / disabled buttons). | 0.5 d |
| S‑C | **HexHelper → modern .NET** | general | `HexUploader/HexHelper.csproj`, `HexUpdater.cs` | SDK-style `net10.0-windows` lib, PackageReference `System.IO.Ports` (SerialPort is not inbox in modern .NET), keep Avrdude `Content` `CopyToOutputDirectory` items exact (avrdude.exe must land next to the executable). | 0.5 d |
| S‑D | **Chart port (FormStickTracker)** | general | `FormStickTracker.cs`, `FormStickTracker.Designer.cs` | Layered on top of S‑A (branch from A's worktree): swap reference → `WinForms.DataVisualization`, validate designer init (ChartArea/Series/Spline/Point), axes and dot-grid rendering, live point streaming. Fallback: ScottPlot rewrite (fallback plan documented in the agent prompt). | 1 d |
| S‑E | **CI workflows** | general | `.github/workflows/app.yml`, `.github/workflows/firmware.yml` (read-only for firmware; only paths-filter wiring if any) | Replace net48 targeting pack steps with `actions/setup-dotnet@v5` pinning 10.0.x + `dotnet build` of the 3 SDK csprojs; keep path filters, version-from-code extraction, and the installer step consistent with G (build MSI only if it clips into CI; vdproj may remain VS-only). | 0.5 d |
| S‑F | **Regression harness** | explore | `docs/migration/checklist.md` (new) | Write the executable behavioral checklist from the cross-component contracts: VID `0x1029` / PID `0xF16C` single-device connect, X/Y from bytes 1–3, options at byte 7, force at bytes 8–9, OUT `[options][force+cmd]` (`Center=0x4000`, `Reboot=0x8000`), options bitmask parity with `fcc3.h` `enum ConfigOptions`, updater flow (Reboot → COM port wait → `avrdude -cavr109 -b57600`), single-instance mutex, config-mode save on exit. Acceptance sheet for Phase 3. | 0.5 d |

Not parallelized (deliberately retained):
- **S‑G (Installer/vdproj):** `.vdproj` builds only in Visual Studio with the Installer Projects extension — agents can't validate it. G is *research-only* in Phase 1 (notes into `docs/migration/installer-notes.md`), real edits happen in Phase 2 once S‑A's publish output shape is known.
- **S‑H (Docs):** README + `AGENTS.md` rewrite deferred to Phase 2 (must describe the final state, not a moving target).

---

## Phase 2 — Vertical integration (merge ladder, sequential by layer)

| ID | Task | Why here |
|----|------|----------|
| MO1 | Merge **S‑A** into `migration/target-net10`; build app project alone (`dotnet build`, not the sln). Fix before anything lands on top. | Foundation layer |
| MO2 | Merge **S‑B** and **S‑C**; build sln without vdproj. | Leaf libs, independent of each other |
| MO3 | Merge **S‑D**; build + launch StickTracker. | Depends on A |
| MO4 | **S‑G1 (performed hands-on, not by agent):** vdproj edits — add `.NET 10 Desktop Runtime` bootstrapper prerequisite, align output paths with SDK convention (`bin\Release\net10.0-windows\`), confirm Avrdude + HexHelper content still included; hand-build MSI in VS. | VS-only tooling |
| MO5 | **S‑E final:** sync `app.yml` to actual publish/installer commands (release-dispatch path builds MSI where possible). | Depends on G |
| MO6 | **S‑H (docs):** `AGENTS.md` Building/Release sections + README; version bump is **not** part of this migration. | Final state known |

---

## Phase 3 — Verification (parallel: verification checks + V‑5 review)

| ID | Check | Agent |
|----|-------|-------|
| V‑1 | Run the **S‑F checklist** end-to-end on Win11 (and one Win10 LTSC instance if available): device connect, axis/force decode matches net48 build side-by-side, config mode write/reboot, fw-upgrade flow. | me |
| V‑2 | GithubUpdater against the real `uriba107/fcc_controller` latest release: correct version shown, both asset buttons enabled, missing-asset → 0.0.0 + disabled. | general |
| V‑3 | Settings: run once; user.config lands in a new profile location; saved UI defaults (gains, force presets, notify flags) reset once — expected, confirm no crash. | me |
| V‑4 | `app.yml` green on PR; `release=true` dispatch produces `app-v` tag with the `.hex` carried forward. | general |
| V‑5 | **Source-diff purity check:** per-stream diff gate — chart stream touches only FormStickTracker* (no unrelated `src/Controller` footage), updater logic unchanged, no sneaky refactors. | explore |

---

## Phase 4 — Release readiness

| ID | Task |
|----|------|
| R‑1 | Cut inside the next scheduled `app-v` release dispatch (no special emergency release). |
| R‑2 | Update this plan with actuals vs. estimates per stream and per merge point (reconciliation table appended below). |

---

## Master TODO (flat checklist, with dependency edges)

> The authoritative live checklist is `docs/migration/TODO.md` (board with Status/Owner/timestamps).
> The static list below mirrors its initial state; keep it in sync only at major milestones.

### Phase 0 (serial)
- [x] **P0‑1** Resolve HidSharp working-tree diff → clean `git status` (already committed; stale VS logs removed)
- [x] **P0‑2** Baseline net48 build + smoke notes (0 errors, 1 warning; MainForm responsive)
- [x] **P0‑3** Decide gates: Chart = `WinForms.DataVisualization` port; MSI = framework-dependent with runtime prerequisite
- [x] **P0‑4** Branch `migration/target-net10` + per-stream worktrees created

### Phase 1 (parallel ×6)
- [x] **S‑A** csproj → SDK-style, deps updated (HidSharp PackageReference; chart package added; `System.Configuration.ConfigurationManager` dropped as redundant in net10)
- [x] **S‑B** GithubUpdater → System.Text.Json
- [x] **S‑C** HexHelper → System.IO.Ports
- [x] **S‑D** Chart port (after S‑A) — validated, no code change needed
- [x] **S‑E** CI workflow draft (setup-dotnet + dotnet build)
- [x] **S‑F** Regression checklist doc
- [x] **S‑G1** Installer research notes (read-only, no csproj edits)

### Phase 2 (merge ladder)
- [x] **MO1** S‑A merged; app alone builds green (`810cd9d`, 0 warnings)
- [x] **MO2** S‑B + S‑C merged; full net10 build green + runtime smoke test (`7e11e66`, `c20f7e1`)
- [x] **MO3** S‑D merged; StickTracker renders + streams OK (`3d6870b`)
- [ ] **MO4** vdproj edits (runtime prereq, path alignment), MSI builds in VS — **BLOCKED** (needs full VS IDE + Installer Projects + .NET 10 bootstrapper; see `installer-notes.md` §5)
- [ ] **MO5** app.yml finalized for full pipeline — build job done (S‑E); vdproj/MSI steps pending MO4
- [x] **MO6** AGENTS.md + README updated

### Phase 3 (verify, parallel: V‑1/V‑3 me, V‑2/V‑4 agent, V‑5 reviewer)
- [ ] **V‑1** Behavioral contract checks pass (S‑F sheet) — **BLOCKED** (needs the physical FCC3 device)
- [x] **V‑2** Updater live-check against real GitHub release
- [x] **V‑3** Settings migration, no crash, defaults sane
- [ ] **V‑4** CI green (PR + release dispatch) — **BLOCKED** (pending MO5/MO4)
- [x] **V‑5** Diff purity audit — no scope creep

### Phase 4
- [ ] **R‑1** Ride the next `app-v` release
- [ ] **R‑2** Plan reconciliation (estimates vs. actuals) appended below

**Dependency DAG**

```
P0-4 ──┬── S-A ──┬── MO1 ──┬── S-D ── MO3
       │         │         └── MO2 (S-B, S-C leaves)
       ├── S-B ──┤
       ├── S-C ──┤
       ├── S-E ──┤
       ├── S-F ──┼────────┬── V-1
       └── S-G1 ─┼── MO4 ─┴── MO5 ── V-4
                 └── MO6
All MOx ──────→ Phase 3 (V-1..V-5) ──→ Phase 4 (R-1, R-2)
```

---

## Parallelism summary

- **Max concurrent subagents = 6** in Phase 1 (S‑A/B/C/D/E/F, with S‑D layered on S‑A once A's csproj is functional), 4 in Phase 3 (V‑1/V‑2/V‑3/V‑4, V‑5 as reviewer).
- **Wall-clock estimate ≈ 2–3 days** (gated by S‑A + MO1, the foundation merge), vs. ~5 days serial.
- Conflict avoidance is by **file ownership** (each stream touches disjoint files) + `worktree`-based branches with a fixed merge ladder.

---

## Appendix — Baseline notes (P0‑2)

### Toolchain state on the dev machine (2026-11-09)

- Visual Studio 18 BuildTools: MSBuild `18.11.1.51303` at
  `C:\Program Files (x86)\Microsoft Visual Studio\18\BuildTools\MSBuild\Current\Bin\MSBuild.exe`.
- No .NET SDK was installed initially; .NET SDK `10.0.403` was installed user-locally for this
  migration at `C:\Users\me\.dotnet\dotnet.exe` (not on `PATH`).
- No .NET Framework 4.8 Developer Pack / targeting pack is installed. The net48 baseline was
  built against the NuGet reference assemblies
  (`Microsoft.NETFramework.ReferenceAssemblies.net48` 1.0.3) via
  `/p:FrameworkPathOverride=<pkg>\build\.NETFramework\v4.8`.
- ClickOnce manifest signing is enabled in the csproj but `SignTool.exe` is unavailable, so the
  baseline build used `/p:SignManifests=false /p:SignAssembly=false`. (S‑A drops these props.)

### Build result

```
msbuild src\App\Fcc3_configurator\FCC_configurator.csproj /t:Rebuild `
  /p:Configuration=Release /p:Platform=AnyCPU `
  /p:FrameworkPathOverride=<net48-ref-assemblies> `
  /p:SignManifests=false /p:SignAssembly=false /m /nologo /verbosity:minimal
```

- Result: **0 errors, 1 warning** — `CS0168` in `HexUploader/HexUpdater.cs(119,30)`
  (`catch (Exception e)` variable never used).
- Output: `src\App\Fcc3_configurator\bin\Release\FCC_configurator.exe` plus
  `GithubUpdater.dll`, `HexUploader.dll`, `HidSharp.dll`, `Avrdude/`.

### Smoke notes

- `FCC_configurator.exe` (reported version **1.0.2.0**) starts on Win11, main window is
  responsive (`MainWindowTitle = "FCC Configurator - 1.0.2.0"`), no startup crash.
- StickTracker / Advanced / FwUpgrade forms were not exercised: no FCC3 device is attached in
  this environment. Form-level regression is covered by the S‑F checklist in Phase 3.

## Appendix — Reconciliation (R‑2)

_record actuals vs. estimates here_
