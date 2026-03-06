# QaaS Mocker Communication Objects

Shared contracts and routing helpers used as the communication layer between `QaaS.Mocker` and `QaaS.Runner`.

[![CI](https://img.shields.io/github/actions/workflow/status/TheSmokeTeam/Qaas.Mocker.CommunicationObjects/ci.yml?branch=master&label=CI&logo=github)](./.github/workflows/ci.yml)
[![NuGet Version](https://img.shields.io/nuget/v/QaaS.Mocker.CommunicationObjects?logo=nuget&label=NuGet)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects)
[![NuGet Downloads](https://img.shields.io/nuget/dt/QaaS.Mocker.CommunicationObjects?logo=nuget&label=Downloads)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects)
[![Qaas.Mocker.CommunicationObjects coverage](https://img.shields.io/badge/Qaas.Mocker.CommunicationObjects%20coverage-100%25-brightgreen)](#test-coverage)
[![Qaas.Mocker.CommunicationObjects.Tests coverage](https://img.shields.io/badge/Qaas.Mocker.CommunicationObjects.Tests%20coverage-98.18%25-yellowgreen)](#test-coverage)

## Solution Overview

This repository contains one solution:

- `Qaas.Mocker.CommunicationObjects.sln`

Projects in the solution:

| Project | Type | Purpose | NuGet |
|---|---|---|---|
| `Qaas.Mocker.CommunicationObjects` | Class Library (`net10.0`) | Shared DTOs/enums and deterministic channel naming helpers for runner-mocker communication | `QaaS.Mocker.CommunicationObjects` |
| `Qaas.Mocker.CommunicationObjects.Tests` | NUnit Test Project (`net10.0`) | Unit tests for routing and command payload behavior | Not packable |

## Documentation

- Product docs: [QaaS Documentation](https://thesmoketeam.github.io/qaas-docs/)
- CI workflow: [`.github/workflows/ci.yml`](./.github/workflows/ci.yml)

## Installation

```xml
<ItemGroup>
  <PackageReference Include="QaaS.Mocker.CommunicationObjects" Version="*" />
</ItemGroup>
```

Dependency note:

- This package depends on `QaaS.Framework.SDK` (currently `1.1.0-alpha.3` in this repository).

## Functionality

### 1) Routing Name Builders (`CommunicationMethods`)

`CommunicationMethods` provides centralized naming rules so both producer and consumer sides generate identical routing keys and queue endpoint names.

- `CreateChannelRunnerToMocker(contentType, serverName?, serverInstanceName?)`
- `CreateChannelMockerToRunner(contentType, serverName?, serverInstanceName?)`
- `CreateConsumerEndpointInput(serverName)`
- `CreateConsumerEndpointOutput(serverName)`

Behavior:

- All generated segments are normalized to lowercase.
- Optional segments are appended in order and kept deterministic.
- Empty string inputs are preserved as empty path segments (verified by tests).

### 2) Command Contracts (`ConfigurationObjects/Command`)

These records and enums model command requests/responses exchanged across services:

- `CommandRequest`
  - `Id`
  - `Command` (`ChangeActionStub`, `TriggerAction`, `Consume`)
  - command-specific payload:
    - `ChangeActionStub`
    - `TriggerAction`
    - `Consume`
  - `AppendObjectToRelevantCommandConfig(object config)` assigns the matching payload object according to `Command`.
- `CommandResponse`
  - `Id`
  - `ServerInstanceId`
  - `Command`
  - `Status` (`Succeeded`, `Failed`)
  - `ExceptionMessage`

Command payload types:

- `ChangeActionStub`: action + stub names
- `TriggerAction`: action name + timeout
- `Consume`: timeout, optional action name, and input/output `DataFilter` values

### 3) Ping Contracts (`ConfigurationObjects/Ping`)

- `PingRequest` contains a request `Id`
- `PingResponse` returns:
  - `Id`
  - `ServerName`
  - `ServerInstanceId`
  - `ServerInputOutputState` (`InputOutputState` enum)

### 4) Input/Output State Enum

`InputOutputState` provides explicit capability flags:

- `NoInputOutput`
- `OnlyInput`
- `OnlyOutput`
- `BothInputOutput`

## Usage Example

```csharp
using Qaas.Mocker.CommunicationObjects;
using Qaas.Mocker.CommunicationObjects.ConfigurationObjects.Command;

var routingKey = CommunicationMethods.CreateChannelRunnerToMocker(
    contentType: "command",
    serverName: "Payments",
    serverInstanceName: "Instance-01");
// runner-to-mocker:command:payments:instance-01

var command = new CommandRequest
{
    Id = "req-42",
    Command = CommandType.TriggerAction
};

command.AppendObjectToRelevantCommandConfig(new TriggerAction
{
    ActionName = "ApproveTransaction",
    TimeoutMs = 2000
});
```

## Test Coverage

Coverage was collected on **March 6, 2026** using:

```powershell
dotnet test Qaas.Mocker.CommunicationObjects.sln --configuration Release --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```

| Project | Line Coverage | Branch Coverage |
|---|---:|---:|
| `Qaas.Mocker.CommunicationObjects` | `100.00%` | `100.00%` |
| `Qaas.Mocker.CommunicationObjects.Tests` | `98.18%` | `100.00%` |

Notes:

- The uncovered line in test project coverage is the auto-generated `Microsoft.NET.Test.Sdk.Program.cs` entrypoint.
- Most DTO records in the library are intentionally marked with `[ExcludeFromCodeCoverage]` to focus metrics on behavioral logic.

## Build, Test, and Pack

```powershell
dotnet restore Qaas.Mocker.CommunicationObjects.sln
dotnet build Qaas.Mocker.CommunicationObjects.sln --configuration Release --no-restore
dotnet test Qaas.Mocker.CommunicationObjects.sln --configuration Release --no-build
```

Package creation from tags is handled by CI:

- Supports tags: `vX.Y.Z`, `vX.Y.Z-alpha.N`, `vX.Y.Z-beta.N`
- Publishes with `NUGET_AUTH_TOKEN`

## Repository Layout

```text
.
|- Qaas.Mocker.CommunicationObjects.sln
|- Qaas.Mocker.CommunicationObjects/
|  |- CommunicationMethods.cs
|  `- ConfigurationObjects/
|     |- Command/
|     `- Ping/
`- Qaas.Mocker.CommunicationObjects.Tests/
```
