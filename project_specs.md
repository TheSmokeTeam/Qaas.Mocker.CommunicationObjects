# project_specs.md — Qaas.Mocker.CommunicationObjects

> Architectural specification for the wire contract between QaaS components.
> See `CLAUDE.md` for the AI operating manual.

## 1. Purpose

Defines the **wire contract** for runtime communication between
`QaaS.Mocker` (controller side) and any client wanting to drive it. The
canonical client is `QaaS.Runner` via its `MockerCommands` action type,
but the contract is transport-agnostic.

## 2. Scope and non-goals

In scope:

- DTOs (request envelopes, response envelopes, command payloads).
- JSON (de)serialisation conventions and converters.
- Deterministic channel-name construction (`CommunicationMethods`).
- Round-trip tests proving wire stability.

Out of scope:

- Transport (Redis, RabbitMQ, etc.) — owned by `QaaS.Mocker.Controller`
  and the runner's `MockerCommands`.
- Validation of business semantics (does this stub exist? is the action
  configured?) — the controller validates on receipt.

## 3. Project structure

| Project | Role |
|---|---|
| `Qaas.Mocker.CommunicationObjects` | Pure contracts. |
| `Qaas.Mocker.CommunicationObjects.Tests` | NUnit round-trip tests. |

Target framework: `.NET 10.0`. Serialisation uses **`System.Text.Json`**
(BCL); no `Newtonsoft.Json` reference.

Source folders:

- `CommunicationMethods.cs` — channel + endpoint name helper.
- `ConfigurationObjects/Command/` — command envelopes, command-type and
  status enums, per-command payload records (`ChangeActionStub`,
  `TriggerAction`, `Consume`).
- `ConfigurationObjects/Ping/` — `PingRequest`, `PingResponse`.

## 4. Public surface

### 4.1 Channel-name helper

`CommunicationMethods` is the **only** authority for channel and endpoint
names. All outputs are lowercased.

```
runner-to-mocker:{contentType}[:{serverName}[:{serverInstanceName}]]
mocker-to-runner:{contentType}[:{serverName}[:{serverInstanceName}]]
```

`contentType` distinguishes payload kind (e.g. `ping`, `command`). The
`serverName` and `serverInstanceName` segments are appended only when the
caller targets a specific server.

Endpoint helpers:

```
{serverName}:input
{serverName}:output
```

### 4.2 Command envelopes

`CommandRequest` (record, `System.Text.Json`):

| Field | Type | Notes |
|---|---|---|
| `Id` | `string` | Caller-generated correlation id (free-form, not a `Guid`). |
| `Command` | `CommandType` (enum) | Serialised as string via `JsonStringEnumConverter`. |
| `ChangeActionStub` | `ChangeActionStub?` | Set iff `Command = ChangeActionStub`. |
| `Consume` | `Consume?` | Set iff `Command = Consume`. |
| `TriggerAction` | `TriggerAction?` | Set iff `Command = TriggerAction`. |

`CommandResponse` (record):

| Field | Type | Notes |
|---|---|---|
| `Id` | `string` | Echo of the request id. |
| `ServerInstanceId` | `string` | Identifies the responding mocker server instance. |
| `Command` | `CommandType` | Echoes the request. |
| `Status` | `Status` (enum) | `Succeeded | Failed`. |
| `ExceptionMessage` | `string?` | Non-null on `Failed`. |

### 4.3 Enums

- `CommandType { ChangeActionStub, TriggerAction, Consume }` — append-only.
- `Status { Succeeded, Failed }` — append-only.

### 4.4 Per-command payloads

| Command | Payload |
|---|---|
| `ChangeActionStub` | `ChangeActionStub` record (replace processor on a stub). |
| `TriggerAction` | `TriggerAction` record (fire a configured action). |
| `Consume` | `Consume` record (synchronously pull a message). |
| `Ping` (separate envelope, not a `CommandType`) | `PingRequest` / `PingResponse`. |

## 5. Wire stability rules

1. Property names are PascalCase on the wire; matching is case-insensitive
   by `System.Text.Json` default options used at both ends.
2. Never reorder enum values — only append.
3. Never rename an enum value without a custom `JsonConverter` aliasing.
4. Never remove a property without a deprecation cycle; treat removal as
   a major-version bump.
5. New optional properties default to `null` / zero; existing clients must
   continue to deserialise successfully.
6. `CommandRequest` exposes one nullable payload property per `CommandType`.
   Adding a new command means adding a new nullable property and a new enum
   member — never repurpose an existing slot.

## 6. Build, packaging, CI

- NuGet identity: `Qaas.Mocker.CommunicationObjects`. Version moves in
  lockstep with Mocker and Runner.
- CI: standard solution pipeline (restore → build → test → coverage →
  pack-and-push on stable tags). Coverage target ≥ 70 %.

## 7. Quality requirements

- Every DTO has a JSON round-trip test.
- Every command type appears in the channel-name test grid.
- No I/O of any kind (no `HttpClient`, no `IConnectionMultiplexer`).
- No reflection over `QaaS.Mocker` / `QaaS.Runner` internal types.

## 8. Compatibility & versioning

A change here is a wire change. Coordinate releases across:

- `QaaS.Mocker` (controller side).
- `QaaS.Runner` (`MockerCommands` action).
- Any external client (downstream tools, dashboards).

## 9. References

- Live docs: <https://docs.qaas.online/mocker/configurationSections/controller/overview/>
