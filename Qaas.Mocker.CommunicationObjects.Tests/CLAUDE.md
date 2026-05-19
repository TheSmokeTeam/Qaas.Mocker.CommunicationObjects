# CLAUDE.md — Qaas.Mocker.CommunicationObjects.Tests

> Test project for the wire-contract DTOs. See repo root `CLAUDE.md`.

## Purpose

Lock the JSON wire shape. Every public DTO in
`Qaas.Mocker.CommunicationObjects` must have a serialise → deserialise →
equality round-trip test, and every channel-name helper must have a
shape-grammar test.

## Layout

- `CommandRequestTests.cs` — round-trip for `CommandRequest` /
  `CommandResponse` across all `CommandType` variants
  (`ChangeActionStub`, `TriggerAction`, `Consume`).
- `CommunicationMethodsTests.cs` — covers the
  `runner-to-mocker:{contentType}[:server[:instance]]` and
  `mocker-to-runner:...` grammars and consumer-endpoint helpers.

## Conventions

- **NUnit** (matches the rest of the QaaS .NET 10 stack — see csproj).
- One test class per source type; mirror the source folder layout.
- Round-trip pattern: build a populated record, `JsonSerializer.Serialize`,
  `JsonSerializer.Deserialize`, assert structural equality (records give
  this for free).
- Enum-on-the-wire assertions check the **string** form, not the int.
- Channel-name tests must enforce lowercase output.
- No mocking framework — DTOs have no collaborators.

## Forbidden

1. `[Test(Ignore = ...)]` / `[Explicit]` to dodge a red test.
2. `Newtonsoft.Json` — assert against `System.Text.Json` only.
3. Asserting on `int` for an enum that is wire-serialised as string.
4. Sharing mutable fixture state between tests.
5. Adding a DTO test that doesn't include a round-trip assertion.

## Run

```bash
dotnet test ../Qaas.Mocker.CommunicationObjects.sln --nologo --no-build
```

When adding a new DTO or enum value, add the corresponding round-trip
test in the same PR — a missing test for a new wire field is a blocker.
