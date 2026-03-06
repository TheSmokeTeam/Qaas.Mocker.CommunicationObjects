# QaaS.Mocker.CommunicationObjects

Shared contracts and routing helpers used as the communication layer between `QaaS.Mocker` and `QaaS.Runner`.

[![CI](https://img.shields.io/badge/CI-GitHub_Actions-2088FF)](./.github/workflows/ci.yml)
[![Docs](https://img.shields.io/badge/docs-qaas--docs-blue)](https://thesmoketeam.github.io/qaas-docs/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)

## Contents
- [Overview](#overview)
- [Packages](#packages)
- [Functionalities](#functionalities)
- [Quick Start](#quick-start)
- [Build and Test](#build-and-test)
- [Documentation](#documentation)

## Overview
This repository contains one solution: [`Qaas.Mocker.CommunicationObjects.sln`](./Qaas.Mocker.CommunicationObjects.sln).

The solution includes:

- `Qaas.Mocker.CommunicationObjects` (`net10.0`): shared communication contracts and deterministic routing key builders.
- `Qaas.Mocker.CommunicationObjects.Tests` (`net10.0`): NUnit tests for routing behavior and command request mapping.

## Packages
| Package | Latest Version | Total Downloads |
|---|---|---|
| [QaaS.Mocker.CommunicationObjects](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) | [![NuGet](https://img.shields.io/nuget/v/QaaS.Mocker.CommunicationObjects?logo=nuget)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) | [![Downloads](https://img.shields.io/nuget/dt/QaaS.Mocker.CommunicationObjects?logo=nuget)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) |

## Functionalities
### Communication Routing (`CommunicationMethods`)
- Generates routing keys for runner-to-mocker traffic via `CreateChannelRunnerToMocker`.
- Generates routing keys for mocker-to-runner traffic via `CreateChannelMockerToRunner`.
- Builds consumer endpoint names via `CreateConsumerEndpointInput` and `CreateConsumerEndpointOutput`.
- Normalizes all segments to lowercase to keep routing deterministic across producers/consumers.

### Command Contracts (`ConfigurationObjects/Command`)
- `CommandRequest` defines command dispatch payload with:
  - `Id`
  - `Command` (`ChangeActionStub`, `TriggerAction`, `Consume`)
  - One command-specific payload object
- `AppendObjectToRelevantCommandConfig` maps an object to the correct command payload based on `Command`.
- `CommandResponse` returns command execution status with:
  - `Id`
  - `ServerInstanceId`
  - `Command`
  - `Status` (`Succeeded` or `Failed`)
  - Optional `ExceptionMessage`

### Command Payload Types
- `ChangeActionStub`: identifies an action and stub pairing to change.
- `TriggerAction`: identifies an action and optional timeout window in milliseconds.
- `Consume`: defines timeout and optional action name with input/output `DataFilter` controls.

### Ping Contracts (`ConfigurationObjects/Ping`)
- `PingRequest` carries a request id.
- `PingResponse` returns:
  - `Id`
  - `ServerName`
  - `ServerInstanceId`
  - `ServerInputOutputState` (`InputOutputState`)

### Input/Output Capability Enum
`InputOutputState` supports:

- `NoInputOutput`
- `OnlyInput`
- `OnlyOutput`
- `BothInputOutput`

## Quick Start
Install package:

```bash
dotnet add package QaaS.Mocker.CommunicationObjects
```

Use routing helpers and command contracts:

```csharp
using Qaas.Mocker.CommunicationObjects;
using Qaas.Mocker.CommunicationObjects.ConfigurationObjects.Command;

var routingKey = CommunicationMethods.CreateChannelRunnerToMocker(
    contentType: "command",
    serverName: "Payments",
    serverInstanceName: "Instance-01");
// runner-to-mocker:command:payments:instance-01

var request = new CommandRequest
{
    Id = "req-42",
    Command = CommandType.TriggerAction
};

request.AppendObjectToRelevantCommandConfig(new TriggerAction
{
    ActionName = "ApproveTransaction",
    TimeoutMs = 2000
});
```

## Build and Test
```bash
dotnet restore Qaas.Mocker.CommunicationObjects.sln
dotnet build Qaas.Mocker.CommunicationObjects.sln -c Release --no-restore
dotnet test Qaas.Mocker.CommunicationObjects.sln -c Release --no-build
```

NuGet publish behavior in CI:

- `dotnet pack` and `dotnet nuget push` run on tags only.
- Supported tag formats:
  - `vX.Y.Z`
  - `vX.Y.Z-alpha.N`
  - `vX.Y.Z-beta.N`

## Documentation
- Official docs: [thesmoketeam.github.io/qaas-docs](https://thesmoketeam.github.io/qaas-docs/)
- CI workflow: [`.github/workflows/ci.yml`](./.github/workflows/ci.yml)
