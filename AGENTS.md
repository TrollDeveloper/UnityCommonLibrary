# Repository working rules

## Build outputs stay local by default

- Keep generated APK/AAB/IPA files, executables, app bundles, installers and build archives local. Do not commit or push them to Git, or automatically upload them as GitHub Actions artifacts or release assets. Do not force-add ignored build outputs.
- Continue running builds, tests and validation. Source code, authored assets, quiz/content images, required dependency binaries and intentional review evidence remain eligible for source control. Keep build archives in ignored build-output directories rather than applying a blanket archive or binary ignore to source assets.
- Upload a specific build output only when the user explicitly requests that output and destination for the current task.
