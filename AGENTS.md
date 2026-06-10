# AGENTS.md — Qaas.Mocker.CommunicationObjects
Guidance for AI agents working in this repository.

## What this repo is
Pure passive DTO/contract library that defines the entire control-plane communication layer between **QaaS.Runner** and **QaaS.Mocker** over Redis pub/sub. Contains zero I/O code — only C# record/class DTOs, an enum `CommandType` (ChangeActionStub | TriggerAction | Consume), and the deterministic channel-name builder `CommunicationMethods`. All serialization is `System.Text.Json` with `[JsonConverter(typeof(JsonStringEnumConverter))]` on enums; `Newtonsoft.Json` is forbidden.

## Projects / Layout

| Project | Type | Purpose |
|---|---|---|
| `Qaas.Mocker.CommunicationObjects/` | Class library (NuGet: `QaaS.Mocker.CommunicationObjects`) | DTOs, enums, `CommunicationMethods` channel builder |
| `Qaas.Mocker.CommunicationObjects.Tests/` | NUnit test project | Channel name roundtrip + JSON serialization tests |

## Build & test
```
dotnet build
dotnet test
dotnet pack -p:PackageVersion=<version>
```
CI runs on `windows-latest`. Coverage measured with `dotnet-coverage collect`; threshold 70%.

## Critical gotchas
- **Version in lockstep with Runner and Mocker.** Any breaking change to a DTO shape or channel-name formula immediately breaks `Runner↔Mocker` wire compatibility. Never rename, add, or remove properties on `CommandRequest`, `CommandResponse`, `PingRequest`, or `PingResponse` without a coordinated semver bump across all three repos.
- **Channel names are fully lowercase.** `CommunicationMethods` calls `.ToLower()` on every segment. Pattern: `runner-to-mocker:{contentType}:{serverName}:{serverInstanceName}` and `mocker-to-runner:{contentType}:{serverName}:{serverInstanceName}`; consumer endpoints: `{serverName}:input` / `{serverName}:output`. Do not hard-code channel strings anywhere — always call `CommunicationMethods`.
- **Only one external dependency:** `QaaS.Framework.SDK`. Do not add transport, Redis, or serialization libraries — this package must remain a lightweight contract-only artifact.
- **`CommandRequest.AppendObjectToRelevantCommandConfig`** dispatches config payloads by `CommandType` enum value; if you add a new command type, add a corresponding branch there and to `CommandType`.
- `Newtonsoft.Json` must NOT be referenced here even indirectly.

## Process
Work follows the QaaS harness pipeline: plan → contract → implement → adversarial evaluation (rubric ≥ 7/10 on Correctness / Completeness / Craft / Robustness). Write tests first (NUnit 4.x, TDD). Use conventional commits (`fix:`, `feat:`, `chore(release):`). Run `dotnet format` before committing.