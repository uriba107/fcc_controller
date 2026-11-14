# Execution Plan: net48 → .NET 10 Migration (FCC3 Configurator App)

**Coordination board:** `docs/migration/TODO.md` — claim tasks (`WIP` + Owner + Started), record
DONE/BLOCKED there, and leave handoff notes for the next agent. This plan is the how; the board
is the current state. Agents may rely only on those two files for state.


**Scope:** `src/App/**` only (FCC_configurator, GithubUpdater, HexHelper, the installer project, app.yml). Firmware (`src/Controller/**`) is explicitly out of scope and untouched. The installer project is being replaced: the VS-only `WindowsInstaller.vdproj` is superseded by a VS-independent WiX project (Decision Gate 2).

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

1. **Chart:** **Port via `WinForms.DataVisualization` NuGet** (API-compatible with the Designer file) — lowest risk. Rewrite (ScottPlot) is the documented fallback if the port fails rendering/regression. *(Decided; S‑D validated, no code change needed.)*
2. **Runtime distribution:** **VS-independent installer via WiX Toolset v5.** Build a framework-dependent MSI from `dotnet publish` output, plus a WiX Burn bootstrapper (`Setup.exe`) that detects and installs the .NET 10 Desktop Runtime before installing the MSI. No Visual Studio, no Installer Projects extension, no VS bootstrapper packages. The Burn bundle becomes the release installer asset; `GithubUpdater` prefers it (`.exe`) with `.msi` as a fallback. **Fallback:** self-contained MSI (runtime bundled, ~150 MB) if the Burn chain proves impractical; this keeps the updater contract unchanged at the cost of a large download.

> **Superseded:** the original gate was "framework-dependent MSI + .NET 10 Desktop Runtime chained via a vdproj bootstrapper prerequisite." That path requires a full Visual Studio 2026 IDE (see below) and is replaced by the WiX path.

### Why Visual Studio was required (and why it is no longer)

The original plan's MO4 was blocked on Visual Studio for two independent reasons:

1. **`.vdproj` is a Visual Studio-only project type.** It is not an MSBuild project and cannot be built by MSBuild or VS BuildTools. It requires `devenv` (the Visual Studio IDE) with the *Microsoft Visual Studio Installer Projects* extension. This machine has VS 18 **BuildTools** only (no `devenv`, no extension), so the MSI could not be built or validated here.
2. **The .NET 10 Desktop Runtime prerequisite ships only with Visual Studio 2026 (18.x).** The prerequisite is a bootstrapper package (`net10desktopruntime_x64`) populated from the VS install's `BootstrapperPackages` folder. VS 2022 17.14 (including the `windows-latest` CI runner) does not include it, and the Installer Projects extension does not ship bootstrapper packages. So even a full VS 2022 install cannot select the .NET 10 runtime prerequisite.

**WiX removes both dependencies.** WiX v5 is a .NET tool / MSBuild SDK: `wix build` (or `dotnet build` of a `.wixproj`) runs on any machine with the .NET SDK, including CI. WiX Burn is a bootstrapper that chains prerequisites itself — `DotNetCoreSearch` detects the .NET 10 Desktop Runtime and an `ExePackage` with a `RemotePayload` (Microsoft URL + SHA-256) installs it. No VS IDE, no Installer Projects extension, no VS bootstrapper package. The result is still a real `.msi` (plus a `Setup.exe` bundle), so Add/Remove Programs and the updater's download-and-launch flow keep working.

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
- **S‑G (Installer/vdproj):** `.vdproj` builds only in Visual Studio with the Installer Projects extension — agents can't validate it. G is *research-only* in Phase 1 (notes into `docs/migration/installer-notes.md`), real edits happen in Phase 2 once S‑A's publish output shape is known. *(Superseded by S‑G2 below: the vdproj path is abandoned in favor of a VS-independent WiX installer.)*
- **S‑H (Docs):** README + `AGENTS.md` rewrite deferred to Phase 2 (must describe the final state, not a moving target).

### Phase 1B — Installer replacement (VS-independent, new)

| ID | Stream | Agent | Files owned (exclusive) | Key work | Est. |
|----|--------|-------|-------------------------|----------|------|
| S‑G2 | **WiX installer (VS-independent)** | general | `src/App/Installer/**` (new `.wixproj`, `Product.wxs`, `Bundle.wxs`, optional `dotnet-tools.json`), `src/App/Fcc3_configurator.sln` (remove the vdproj), `src/App/WindowsInstaller/WindowsInstaller.vdproj` (delete), `src/App/GithubUpdater/GithubUpdater.cs` (asset preference: `.exe` bundle first, `.msi` fallback), `src/App/Fcc3_configurator/MainForm.cs` (only if the launch flow needs a tweak) | Author a WiX v5 installer that builds a framework-dependent MSI from `dotnet publish` output and a Burn `Setup.exe` that chains the .NET 10 Desktop Runtime (`DotNetCoreSearch` + `ExePackage`/`RemotePayload`). Remove the vdproj from the solution and delete it. Update the updater to prefer the Burn bundle asset (`.exe`) and fall back to `.msi` for older releases; keep the version parsing (`fcc_controller_<version>` + extension) and null guards. Validate the MSI + bundle on a clean VM (no .NET 10 runtime) and confirm Avrdude is packaged. | 1 d |

**Dependency:** S‑G2 builds on the merged S‑A/S‑B/S‑C/S‑D state (publish output shape and updater are known). It is the only stream that touches `GithubUpdater.cs` after S‑B; the change is limited to asset preference, not download/parse semantics. The original S‑B constraint "do not change asset-picking logic" is superseded for the installer migration only.

---

## Phase 2 — Vertical integration (merge ladder, sequential by layer)

| ID | Task | Why here |
|----|------|----------|
| MO1 | Merge **S‑A** into `migration/target-net10`; build app project alone (`dotnet build`, not the sln). Fix before anything lands on top. | ✅ Done (`810cd9d`) |
| MO2 | Merge **S‑B** and **S‑C**; build sln without vdproj. | ✅ Done (`7e11e66`, `c20f7e1`) |
| MO3 | Merge **S‑D**; build + launch StickTracker. | ✅ Done (`3d6870b`) |
| MO4 | **S‑G2:** merge the WiX installer stream; build the MSI + Burn bundle from the publish output; validate on a clean VM (runtime chaining, Avrdude, uninstall); re-run the updater live check. | VS-independent installer |
| MO5 | **S‑E final:** replace the `devenv`/`DisableOutOfProcBuild` steps in `app.yml` with the WiX build (`dotnet tool restore` + `wix build`, or `dotnet build` of the `.wixproj`); stage `fcc_controller_<version>.exe` (Burn) and `.msi`; keep the firmware carry-forward. | Depends on MO4 |
| MO6 | **S‑H (docs):** `AGENTS.md` Building/Release sections + README; version bump is **not** part of this migration. | ✅ Done (follow-up: installer references → WiX/`.exe`) |

---

## Phase 3 — Verification (parallel: verification checks + V‑5 review)

| ID | Check | Agent |
|----|-------|-------|
| V‑1 | Run the **S‑F checklist** end-to-end on Win11 (and one Win10 LTSC instance if available): device connect, axis/force decode matches net48 build side-by-side, config mode write/reboot, fw-upgrade flow. **Add installer checks:** clean-VM install via the Burn bundle (runtime chained), Avrdude present, uninstall clean. | me |
| V‑2 | GithubUpdater against the real `uriba107/fcc_controller` latest release: correct version shown, both asset buttons enabled, missing-asset → 0.0.0 + disabled. **Re-run after S‑G2** (asset preference `.exe` → `.msi`). | general |
| V‑3 | Settings: run once; user.config lands in a new profile location; saved UI defaults (gains, force presets, notify flags) reset once — expected, confirm no crash. | me |
| V‑4 | `app.yml` green on PR; `release=true` dispatch produces `app-v` tag with the `.hex` carried forward; the release contains the Burn bundle (and MSI). | general |
| V‑5 | **Source-diff purity check:** per-stream diff gate — chart stream touches only FormStickTracker*, updater change limited to asset preference, no sneaky refactors. | explore |

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
> **Last synced from the board:** 2026-11-14 (Phase 1 + MO1–MO6 done; S‑G2 WiX installer authored, MSI + Burn bundle build and validated locally; V‑2/V‑3/V‑5 done; V‑1/V‑4 blocked).

### Phase 0 (serial)
- [x] **P0‑1** Resolve HidSharp working-tree diff → clean `git status` (already committed; stale VS logs removed)
- [x] **P0‑2** Baseline net48 build + smoke notes (0 errors, 1 warning; MainForm responsive)
- [x] **P0‑3** Decide gates: Chart = `WinForms.DataVisualization` port; runtime distribution = VS-independent WiX installer (framework-dependent MSI + Burn bundle; self-contained MSI fallback)
- [x] **P0‑4** Branch `migration/target-net10` + per-stream worktrees created

### Phase 1 (parallel ×6)
- [x] **S‑A** csproj → SDK-style, deps updated (HidSharp PackageReference; chart package added; `System.Configuration.ConfigurationManager` dropped as redundant in net10)
- [x] **S‑B** GithubUpdater → System.Text.Json
- [x] **S‑C** HexHelper → System.IO.Ports
- [x] **S‑D** Chart port (after S‑A) — validated, no code change needed
- [x] **S‑E** CI workflow draft (setup-dotnet + dotnet build)
- [x] **S‑F** Regression checklist doc
- [x] **S‑G1** Installer research notes (read-only, no csproj edits)

### Phase 1B (installer replacement)
- [x] **S‑G2** VS-independent WiX installer: MSI + Burn bundle from publish output, vdproj removed, updater prefers `.exe` (fallback `.msi`)

### Phase 2 (merge ladder)
- [x] **MO1** S‑A merged; app alone builds green (`810cd9d`, 0 warnings)
- [x] **MO2** S‑B + S‑C merged; full net10 build green + runtime smoke test (`7e11e66`, `c20f7e1`)
- [x] **MO3** S‑D merged; StickTracker renders + streams OK (`3d6870b`)
- [x] **MO4** WiX installer merged; MSI + Burn bundle build and locally validated (MSI admin-install, Avrdude packaged, bundle `/layout` fetches the runtime) — **replaces the VS-only vdproj MO4**. Clean-VM install/uninstall remains in V‑1.
- [x] **MO5** app.yml finalized with the WiX build (no `devenv`); stages `fcc_controller_<version>.exe` + `.msi`
- [x] **MO6** AGENTS.md + README updated (follow-up: installer references → WiX/`.exe`)

### Phase 3 (verify, parallel: V‑1/V‑3 me, V‑2/V‑4 agent, V‑5 reviewer)
- [ ] **V‑1** Behavioral contract checks pass (S‑F sheet) + installer clean-VM checks — **BLOCKED** (needs the physical FCC3 device)
- [x] **V‑2** Updater live-check against real GitHub release — re-run after S‑G2: live picks `.msi` fallback `1.0.2.0`, synthetic `.exe` wins (`9.9.9`), missing-asset guards hold
- [x] **V‑3** Settings migration, no crash, defaults sane
- [ ] **V‑4** CI green (PR + release dispatch) — **BLOCKED** (MO4/MO5 implemented, needs a CI run)
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
       └── S-G2 ─┴── MO4 ──┴── MO5 ── V-4
                 └── MO6 (docs follow-up)
All MOx ──────→ Phase 3 (V-1..V-5) ──→ Phase 4 (R-1, R-2)
```

---

## Parallelism summary

- **Max concurrent subagents = 6** in Phase 1 (S‑A/B/C/D/E/F, with S‑D layered on S‑A once A's csproj is functional), 1 in Phase 1B (S‑G2), 4 in Phase 3 (V‑1/V‑2/V‑3/V‑4, V‑5 as reviewer).
- **Wall-clock estimate ≈ 2–3 days** for the original migration (gated by S‑A + MO1, the foundation merge); the VS-independent installer adds ~1 day (S‑G2 + MO4/MO5).
- Conflict avoidance is by **file ownership** (each stream touches disjoint files) + `worktree`-based branches with a fixed merge ladder.

---

## Appendix — Baseline notes (P0‑2)

### Toolchain state on the dev machine (2026-11-09)

- Visual Studio 18 BuildTools: MSBuild `18.11.1.51303` at
  `C:\Program Files (x86)\Microsoft Visual Studio\18\BuildTools\MSBuild\Current\Bin\MSBuild.exe`.
  Not needed for the installer anymore (WiX is a .NET tool); retained only as the net48
  baseline / reference-assembly build path.
- .NET SDK `10.0.403` is on `PATH` at `C:\Program Files\dotnet\dotnet.exe` (an earlier
  user-local install at `C:\Users\me\.dotnet\dotnet.exe` was used during preflight).
- No WiX Toolset is installed yet. S‑G2 installs it as a .NET tool
  (`dotnet tool install --global wix`, or `dotnet tool restore` from a committed
  `dotnet-tools.json` for CI). Pin **WiX v5** (the last release with public binaries);
  WiX 6+ requires a commercial license for official binaries.
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
