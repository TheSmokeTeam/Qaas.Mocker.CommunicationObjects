# CLAUDE.md — Qaas.Mocker.CommunicationObjects (library)

> Project-level operating manual. See repo root `CLAUDE.md` and
> `project_specs.md` for architecture.

## Purpose

Pure wire-contract DTOs + channel-name helpers shared by `QaaS.Mocker`
and any client driving it (typically `QaaS.Runner`'s `MockerCommands`).
Zero behaviour, zero I/O, zero references to Mocker / Runner / Framework
internals.

## Key files

- `CommunicationMethods.cs` — channel + endpoint name helpers. All
  outputs lowercased; grammar `runner-to-mocker:{contentType}[:{server}[:{instance}]]`
  and `mocker-to-runner:...`.
- `ConfigurationObjects/Command/CommandRequest.cs`,
  `CommandResponse.cs`, `CommandType.cs`, `Status.cs`,
  `ChangeActionStub.cs`, `Consume.cs`, `TriggerAction.cs`.
- `ConfigurationObjects/Ping/PingRequest.cs`, `PingResponse.cs`.

## Conventions

- Records, init-only, nullable enabled.
- Serialisation = `System.Text.Json` only.
- Enums carry `[JsonConverter(typeof(JsonStringEnumConverter))]` so the
  wire shape is the symbol name.
- New enum values are **appended**; never inserted mid-list.
- Channel names are produced exclusively by `CommunicationMethods`;
  callers must not concat strings.

## Forbidden

1. Any `using QaaS.Mocker.*` or `using QaaS.Runner.*`.
2. `Newtonsoft.Json` attributes — wire format is STJ.
3. I/O, logging, DI, threading primitives.
4. Renaming a JSON-serialised property without a wire-compat shim.
5. Inserting an enum value mid-list (only append).

## Build

```bash
dotnet build ../Qaas.Mocker.CommunicationObjects.sln --nologo -clp:ErrorsOnly
csharpier format <changed-files>
```

Tests live in the sibling `Qaas.Mocker.CommunicationObjects.Tests`
project — every public DTO must have a JSON round-trip test there.
