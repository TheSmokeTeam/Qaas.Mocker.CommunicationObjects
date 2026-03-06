# QaaS.Mocker.CommunicationObjects

Shared contracts and routing helpers used as the communication layer between `QaaS.Mocker` and `QaaS.Runner`.

[![CI](https://img.shields.io/badge/CI-GitHub_Actions-2088FF)](./.github/workflows/ci.yml)
[![Docs](https://img.shields.io/badge/docs-qaas--docs-blue)](https://thesmoketeam.github.io/qaas-docs/)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)

## Contents
- [Overview](#overview)
- [Packages](#packages)
- [Architecture](#architecture)
- [Quick Start](#quick-start)
- [Documentation](#documentation)

## Overview
This repository contains one solution: [`Qaas.Mocker.CommunicationObjects.sln`](./Qaas.Mocker.CommunicationObjects.sln).

The solution is intentionally small and focused:

- One NuGet package project for runtime communication contracts.
- One test project for deterministic routing and command mapping behavior.

## Packages
| Package | Latest Version | Total Downloads |
|---|---|---|
| [QaaS.Mocker.CommunicationObjects](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) | [![NuGet](https://img.shields.io/nuget/v/QaaS.Mocker.CommunicationObjects?logo=nuget)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) | [![Downloads](https://img.shields.io/nuget/dt/QaaS.Mocker.CommunicationObjects?logo=nuget)](https://www.nuget.org/packages?q=QaaS.Mocker.CommunicationObjects) |

## Architecture
### [QaaS.Mocker.CommunicationObjects](./Qaas.Mocker.CommunicationObjects/)
- `CommunicationMethods`:
  - Creates runner-to-mocker routing keys.
  - Creates mocker-to-runner routing keys.
  - Creates input/output consumer endpoint names.
  - Enforces lowercase normalization for deterministic channel naming.
- `ConfigurationObjects/Command`:
  - `CommandRequest` and `CommandResponse` transport command execution data.
  - `CommandType` and `Status` define command/state enums.
  - `ChangeActionStub`, `TriggerAction`, and `Consume` define command payload models.
- `ConfigurationObjects/Ping`:
  - `PingRequest` and `PingResponse` for service liveness/capability signaling.
  - `InputOutputState` defines input/output capability states.

### [QaaS.Mocker.CommunicationObjects.Tests](./Qaas.Mocker.CommunicationObjects.Tests/)
- Validates routing generation for all communication helper methods.
- Validates `CommandRequest.AppendObjectToRelevantCommandConfig` mapping behavior.
- Covers valid, unknown-command, and invalid-cast command configuration scenarios.

## Quick Start
Install package:

```bash
dotnet add package QaaS.Mocker.CommunicationObjects
```

Upgrade package:

```bash
dotnet add package QaaS.Mocker.CommunicationObjects --version <TARGET_VERSION>
dotnet restore
```

## Documentation
- Official docs: [thesmoketeam.github.io/qaas-docs](https://thesmoketeam.github.io/qaas-docs/)
- CI workflow: [`.github/workflows/ci.yml`](./.github/workflows/ci.yml)
