# project_specs.md — Qaas.Mocker.CommunicationObjects (package project)

The single contracts assembly. Pure DTOs + helper; zero behaviour.

## Folders

- `CommunicationMethods.cs` — deterministic channel-name builder
  (lowercased; the only authoritative source).
- `ConfigurationObjects/Command/` — command envelopes + payloads
  (`CommandRequest`, `CommandResponse`, `CommandType`, `Status`,
  `ChangeActionStub`, `Consume`, `TriggerAction`).
- `ConfigurationObjects/Ping/` — `PingRequest`, `PingResponse`.

## Forbidden in this project

- Adding any I/O or transport code.
- Constructing a channel name outside `CommunicationMethods`.
- Adding behavioural extension methods (DTOs are passive).
- Mixing `Newtonsoft.Json` attributes — wire format is `System.Text.Json`.
- Referencing any `QaaS.*` package outside the SDK (which is currently not
  required by this assembly at all).

## Tests

`Qaas.Mocker.CommunicationObjects.Tests` — round-trip tests for every
DTO and the channel-name helper.
