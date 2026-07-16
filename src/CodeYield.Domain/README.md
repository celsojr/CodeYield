# CodeYield.Domain

Domain primitives: base entities, value objects, and smart enums for .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.Domain.svg)](https://www.nuget.org/packages/CodeYield.Domain)

## Types

### Base Classes

- `BaseEntity<TId>` — entity with identity equality
- `BaseAggregateRoot<TId>` — aggregate root with domain event collection
- `AuditableEntity<TId>` — entity with `CreatedAt` / `UpdatedAt` tracking

### Value Objects

- `ValueObject` — base class with `GetEqualityComponents()`
- `ValueObject<T>` — single-component value object
- `Email` — validated email address
- `Money` — amount + currency with arithmetic
- `Address` — street, city, state, zip, country

### Smart Enums

- `Enumeration<TEnum>` — type-safe enum with `FromValue()`, `FromName()`, `GetAll()`

## Install

```bash
dotnet add package CodeYield.Domain
```

## License

MIT
