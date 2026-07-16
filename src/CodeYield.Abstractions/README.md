# CodeYield.Abstractions

Core interfaces for Domain-Driven Design in .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.Abstractions.svg)](https://www.nuget.org/packages/CodeYield.Abstractions)

## Interfaces

| Interface | Purpose |
|---|---|
| `IEntity<TId>` | Entity with a typed identifier |
| `IAggregateRoot` | Marker for aggregate roots |
| `IDomainEvent` | Domain event contract |
| `IEditableEntity<TId>` | Entity that supports update tracking |
| `ITenantContext` | Multi-tenant context provider |

## Install

```bash
dotnet add package CodeYield.Abstractions
```

## License

MIT
