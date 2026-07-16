# CodeYield.Exceptions

Typed exception hierarchy for Domain-Driven Design in .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.Exceptions.svg)](https://www.nuget.org/packages/CodeYield.Exceptions)

## Exceptions

| Exception | Use Case |
|---|---|
| `DomainException` | Business rule violation |
| `NotFoundException` | Entity not found |
| `ValidationException` | Input validation failure |

```csharp
throw new NotFoundException("Product", productId);

throw new ValidationException("Name is required.", "Price must be positive.");

throw new DomainException("Cannot ship an order that hasn't been confirmed.");
```

## Install

```bash
dotnet add package CodeYield.Exceptions
```

## License

MIT
