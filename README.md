# QaaS.Mocker.CommunicationObjects

Shared communication contracts and routing helpers used as the mediation layer between `QaaS.Mocker` and `QaaS.Runner`.

## Target Framework

- C# 14
- .NET 10 (`net10.0`)

## Package & Dependencies

- NuGet package id: `QaaS.Mocker.CommunicationObjects`
- Primary dependency: `QaaS.Framework.SDK` `1.0.0`

## CI and NuGet Publishing

- CI workflow: `.github/workflows/ci.yml`
- Build/test run on every push and pull request.
- NuGet publish runs only for Git tags and requires `NUGET_AUTH_TOKEN` secret.
- Supported release tags:
  - `v1.2.3`
  - `v1.2.3-alpha.1`
  - `v1.2.3-beta.1`
