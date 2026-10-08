# Build outputs stay local by default — 2026-10-08

## Request and scope
- Keep generated APK/AAB/IPA, executables, app bundles, installers and build archives local unless a specific upload is explicitly requested.
- Update Git exclusions and persistent work instructions. Preserve source, required dependency binaries, authored/content images and intentional review evidence.

## Changes
- Added missing build-output exclusion patterns to `.gitignore`; ordinary ZIP archives and dependency executables under source directories are not blanket-ignored.
- Added the local-only default to `AGENTS.md`.
- Existing workflow definitions remain unchanged; this repository has no automatic build-output artifact upload in its current default branch.

## Verification
- YAML syntax parsing passed for all 0 existing workflow files.
- `git check-ignore --no-index --stdin`: 21 build-output cases excluded; 10 source/asset/review cases remained eligible.
- Existing 32 tracked paths retained their prior ignore status, and no tracked file was removed.
- `git diff --check` passed. No application code changed; application tests, builds and manual Actions runs were not rerun for this configuration-only change.

## Git handoff and remaining limits
- Started from `d7e2190667804842be8c18e211566e2de7db6b6d` in an isolated sparse clone; pre-edit pull succeeded with no conflicts or user changes. Implemented on `codex/no-build-artifact-upload`.
- Integration/push result and final commit are reported after the operation; this entry does not preclaim remote success. Existing unrelated/unpushed work in other checkouts is excluded.
- Existing remote artifacts/releases, Git history, account budgets, permissions and production automation are unchanged. Existing tracking is not removed merely by adding ignore rules.
- A normal push may trigger existing CI checks; the changed CraftingCalculator workflow has no build-artifact upload in that triggering commit.
