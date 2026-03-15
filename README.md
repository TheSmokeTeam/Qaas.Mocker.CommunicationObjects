# QaaS.Mocker.CommunicationObjects

Shared contracts and routing helpers used as the communication layer between `QaaS.Mocker` and `QaaS.Runner`.

[![CI](https://github.com/TheSmokeTeam/Qaas.Mocker.CommunicationObjects/actions/workflows/ci.yml/badge.svg)](https://github.com/TheSmokeTeam/Qaas.Mocker.CommunicationObjects/actions/workflows/ci.yml)
[![Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/eldarush/2aff89de24574268c3ed6bd2ed5f8b83/raw/coverage-badge.json)](https://github.com/TheSmokeTeam/Qaas.Mocker.CommunicationObjects/actions/workflows/ci.yml)
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

The solution is split into:

- One package project with shared communication contracts and deterministic routing key builders.
- One test project validating routing and command payload mapping behavior.

## Packages
| Package | Latest Version | Total Downloads |
|---|---|---|
| [QaaS.Mocker.CommunicationObjects](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) | [![NuGet](https://img.shields.io/nuget/v/QaaS.Mocker.CommunicationObjects?logo=nuget)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) | [![Downloads](https://img.shields.io/nuget/dt/QaaS.Mocker.CommunicationObjects?logo=nuget)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) |

## Functionalities
### [QaaS.Mocker.CommunicationObjects](./Qaas.Mocker.CommunicationObjects/)
- `CommunicationMethods` generates runner-to-mocker and mocker-to-runner channel names.
- `CommunicationMethods` generates input/output consumer endpoint names.
- `CommandRequest` and `CommandResponse` define command exchange payloads.
- `CommandType`, `Status`, and `InputOutputState` define communication state enums.
- `ChangeActionStub`, `TriggerAction`, `Consume`, `PingRequest`, and `PingResponse` define configuration and ping contracts.

### [QaaS.Mocker.CommunicationObjects.Tests](./Qaas.Mocker.CommunicationObjects.Tests/)
- Verifies deterministic route generation and lower-casing behavior.
- Verifies command payload assignment logic in `AppendObjectToRelevantCommandConfig`.
- Verifies behavior for unknown command values and invalid cast scenarios.

## Quick Start
Install package:

```bash
dotnet add package QaaS.Mocker.CommunicationObjects
```

Update package:

```bash
dotnet add package QaaS.Mocker.CommunicationObjects --version <TARGET_VERSION>
dotnet restore
```

## Build and Test
```bash
dotnet restore Qaas.Mocker.CommunicationObjects.sln
dotnet build Qaas.Mocker.CommunicationObjects.sln -c Release --no-restore
dotnet test Qaas.Mocker.CommunicationObjects.sln -c Release --no-restore --maxcpucount
```

## Documentation
- Official docs: [thesmoketeam.github.io/qaas-docs](https://thesmoketeam.github.io/qaas-docs/)
- CI workflow: [`.github/workflows/ci.yml`](./.github/workflows/ci.yml)
