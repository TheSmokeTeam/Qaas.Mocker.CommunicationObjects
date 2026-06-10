# Copilot Instructions — Qaas.Mocker.CommunicationObjects

Read `AGENTS.md` at the repo root first — it is the authoritative reference for this contract library and explains the Runner↔Mocker wire protocol, channel naming rules, and versioning constraints that govern all changes here.

## Essentials
- This is a **pure DTO library**: zero I/O, zero transport — only C# contracts and the `CommunicationMethods` channel builder.
- All serialization uses `System.Text.Json` with string enum converters. Never reference `Newtonsoft.Json`.
- Channel names are always lowercase; always use `CommunicationMethods` — never hard-code channel strings.
- Any breaking DTO or channel change requires a **coordinated version bump** in QaaS.Runner and QaaS.Mocker simultaneously.
- TDD (NUnit 4.x), `dotnet format` clean, conventional commits.