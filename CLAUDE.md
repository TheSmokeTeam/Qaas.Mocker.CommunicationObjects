# CLAUDE.md — Qaas.Mocker.CommunicationObjects

> Operating manual for AI assistants working in the
> **Qaas.Mocker.CommunicationObjects** repository.
> See `project_specs.md` for the architectural spec.
> Live docs: <https://docs.qaas.online/>.

## Mission

Defines the **wire contract** between `QaaS.Mocker` and any client that
wants to drive it remotely (typically `QaaS.Runner`'s `MockerCommands`
action). This package contains nothing but DTOs + a channel-name helper;
it must never reference Mocker, Runner, or Framework internals.

## Build / Test

```bash
dotnet build Qaas.Mocker.CommunicationObjects.sln --nologo -clp:ErrorsOnly
dotnet test  Qaas.Mocker.CommunicationObjects.sln --nologo --no-build
csharpier format <changed-files>
```

## Structure

| Project | Purpose |
|---|---|
| `Qaas.Mocker.CommunicationObjects` | Pure DTOs + channel-name helper. |
| `Qaas.Mocker.CommunicationObjects.Tests` | NUnit round-trip tests. |

Source folder map:

```
Qaas.Mocker.CommunicationObjects/
├── CommunicationMethods.cs            # channel + endpoint name helpers
└── ConfigurationObjects/
    ├── Command/
    │   ├── CommandRequest.cs          # envelope: Id, Command, ChangeActionStub?, Consume?, TriggerAction?
    │   ├── CommandResponse.cs         # envelope: Id, ServerInstanceId, Command, Status, ExceptionMessage?
    │   ├── CommandType.cs             # enum: ChangeActionStub | TriggerAction | Consume
    │   ├── Status.cs                  # enum: Succeeded | Failed
    │   ├── ChangeActionStub.cs        # payload
    │   ├── Consume.cs                 # payload
    │   └── TriggerAction.cs           # payload
    └── Ping/
        ├── PingRequest.cs
        └── PingResponse.cs
```

## Public surface

### Channel-name helpers (`CommunicationMethods`)

```csharp
// All outputs are lowercased.
string CreateChannelRunnerToMocker(string contentType,
    string? serverName = null, string? serverInstanceName = null);
string CreateChannelMockerToRunner(string contentType,
    string? serverName = null, string? serverInstanceName = null);
string CreateConsumerEndpointInput(string serverName);
string CreateConsumerEndpointOutput(string serverName);
```

Channel grammar:

```
runner-to-mocker:{contentType}[:{serverName}[:{serverInstanceName}]]
mocker-to-runner:{contentType}[:{serverName}[:{serverInstanceName}]]
```

Typical `contentType` values used today: `ping`, `command`. Server / instance
segments are appended only when the caller targets a specific server.

### Command envelopes

`CommandRequest` (record):

| Field | Type | Notes |
|---|---|---|
| `Id` | `string` | Caller-generated correlation id. |
| `Command` | `CommandType` (enum) | `ChangeActionStub | TriggerAction | Consume`. Serialised as string via `JsonStringEnumConverter`. |
| `ChangeActionStub` | `ChangeActionStub?` | Populated when `Command = ChangeActionStub`. |
| `Consume` | `Consume?` | Populated when `Command = Consume`. |
| `TriggerAction` | `TriggerAction?` | Populated when `Command = TriggerAction`. |

`CommandResponse` (record):

| Field | Type | Notes |
|---|---|---|
| `Id` | `string` | Mirrors the request id. |
| `ServerInstanceId` | `string` | Identifies which mocker server instance produced the response. |
| `Command` | `CommandType` | Echoes the request. |
| `Status` | `Status` (enum) | `Succeeded | Failed`. |
| `ExceptionMessage` | `string?` | Non-null on `Failed`. |

`PingRequest` / `PingResponse` are simple ack records.

### Serialisation

- **`System.Text.Json`** (not Newtonsoft). Enums use
  `[JsonConverter(typeof(JsonStringEnumConverter))]`.
- Round-trip is the contract — every DTO has a corresponding test in
  `Qaas.Mocker.CommunicationObjects.Tests` proving
  `Deserialize(Serialize(x)) ≡ x`.

## Versioning

This package is consumed by both Runner and Mocker. **Any breaking change**
forces a coordinated release across all three repos. Treat even DTO field
renames as breaking unless the JSON wire shape is preserved (custom
converter or matching property name).

## Forbidden patterns (NEVER do)

1. Reference `QaaS.Mocker.*` or `QaaS.Runner.*`.
2. Add behaviour beyond DTO logic; nothing in here may make I/O.
3. Construct channel names by string concat anywhere outside
   `CommunicationMethods`.
4. Change a JSON property name without a wire-compatibility shim and a
   versioning bump.
5. Add an enum value in the middle of an existing enum (only append).
6. Skip a test rather than fix it.
7. Mix `Newtonsoft.Json` attributes into this assembly — wire format is
   `System.Text.Json`.

## Must-verify before declaring done

1. `dotnet build` / `dotnet test` green.
2. Every DTO has a JSON round-trip test.
3. Every command type appears in the channel-name test grid.
4. Newly-added enum values are appended (never inserted).
5. Wire compat with previous mocker/runner versions confirmed.
6. CI workflow `.github/workflows/ci.yml` is green.
7. Framework SDK reference still pinned to the same version family as
   Mocker / Runner.

## Key files

- `CommunicationMethods.cs` — channel + endpoint name helpers.
- `ConfigurationObjects/Command/CommandRequest.cs`,
  `CommandResponse.cs`, `CommandType.cs`, `Status.cs`.
- `ConfigurationObjects/Ping/PingRequest.cs`, `PingResponse.cs`.
- Tests mirror the source layout one-for-one.
