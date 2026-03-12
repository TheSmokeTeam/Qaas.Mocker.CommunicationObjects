## Summary

Describe the change in 2-4 sentences. Focus on user-visible behavior, dependency updates, or contract changes.

## Why

Explain the problem this PR solves and why this approach was chosen.

## Changes

- List the main code or configuration changes.
- Call out anything intentionally left unchanged.
- Mention dependency version changes explicitly.

## Validation

- [ ] `dotnet restore Qaas.Mocker.CommunicationObjects.sln`
- [ ] `dotnet build Qaas.Mocker.CommunicationObjects.sln --no-restore --configuration Release`
- [ ] `dotnet test Qaas.Mocker.CommunicationObjects.sln --configuration Release --no-restore --maxcpucount`

## Release Impact

- [ ] No release impact
- [ ] Package version or dependency impact
- [ ] CI/workflow impact

Notes:
- Mention whether the package remains stable-only or introduces prerelease dependencies.
- Link any related PRs, issues, or external package releases.
