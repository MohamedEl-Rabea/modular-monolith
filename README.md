# modulith-starter

Private solution template for the team. This folder is meant to be copied into a new repo and then customized.

## High-level structure
- `Api/` — entry API project
- `BuildingBlocks/` — shared building blocks and cross-cutting utilities
- `Modules/` — feature modules (see `Modules/README.md`)
- `Tests/` — architecture tests

## Usage
1. Copy this folder into a new repo.
2. Rename `SampleModule` and update namespaces.
3. Wire module registration in the API host if required.
4. Rename the solution file if desired (`modulith-starter.sln`).

## Notes
- Stubbed code is intentional and for guidance only.
- References to BuildingBlocks assume the host solution includes them.
