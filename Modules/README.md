# Modules

Guidance for creating a new module.

## Suggested structure
- `YourModule/YourModule.Contracts` — public module contracts and shared models
- `YourModule/YourModule.Service` — features, data, integrations, and module registration
- `YourModule/YourModule.Service.Tests` — unit tests for handlers, validators, and services

## Required pieces
- Module registration class (`YourModuleModule.cs`) with `Add...Module` and `Map...Module`.
- Data setup in `Data/Setup.cs` (DbContext, queries, cache, seeders).
- At least one feature endpoint + handler in `Features/.../v1`.
- Tests for handlers and validators.

## Naming
- Use `YourModule` consistently in folder names, namespaces, and project names.
- Keep API route prefixes under `api/v{version}` and tag per feature area.

## Checklist
- Contracts project created and referenced by service.
- Service project references BuildingBlocks and Contracts.
- Tests project references Service.
- Module wired into API host.
