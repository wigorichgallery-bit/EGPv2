# EGPv2 Source Lock — 11 August 2026 21:38 WIB

## Locked source snapshot

- Source archive: `EGPv2_11Agustus2026_2138Wib.zip`
- Archive SHA-256: `005a340f72403600911b0631e5d099c404b484f2d8b7eddb4e38166821312fe8`
- Baseline: `EGPv2_Baseline.txt`
- Extracted snapshot: `/mnt/data/EGPv2_11Agustus2026_2138Wib_extracted`
- Target framework: `net10.0` across all projects
- Projects: **19** (11 production + 8 unit-test)
- C# source files parsed: **594**
- Type declarations parsed: **554**
  - classes: 420
  - interfaces: 73
  - records: 46
  - enums: 15
- Zero-byte `.cs` files: **31**
- `.bak` source artifacts: **6**
- TODO/FIXME/HACK/XXX markers: **0**

## Baseline structure verification

The baseline contains **626** source/test paths under `src/` and `tests/`.
The current archive contains **627** such paths.

- Missing baseline source/test paths: **0**
- Extra source/test path: **1**
  - `tests/Platform.Identity.Application.UnitTests/EGPv2_Baseline.txt`

The baseline's `Docs/*`, previous source ZIP, and architecture artifact entries are not present in the new archive. They are treated as baseline/reference artifacts rather than source-code drift.

## Lock semantics

The complete archive is treated as the immutable source snapshot for this session. Each parsed `.cs` file is also individually content-locked by SHA-256 in `02_Class_Lock_Manifest.json`.

**Important:** an empty file is intentionally locked as empty; it is not silently reconstructed or filled. Any implementation work must be treated as a subsequent change against this locked baseline.
