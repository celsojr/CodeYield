# CodeYield.Mediator

Lightweight CQRS contracts, pipeline behaviors, and an in-process dispatcher for .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.Mediator.svg)](https://www.nuget.org/packages/CodeYield.Mediator)

## Features

- **Commands & Queries** — `ICommand<T>`, `IQuery<T>` with typed handlers
- **Result type** — `Result<T>` for clean success/failure flows
- **Pipeline behaviors** — logging, performance monitoring, validation
- **Domain event handlers** — `IDomainEventHandler<T>`
- **Validators** — `IValidator<T>` with `ValidationResult`
- **MediatR coexistence** — use alongside MediatR with a thin adapter

## Usage

```csharp
// Command
public record CreateProductCommand(string Name, decimal Price) : ICommand<Guid>;

// Handler
public class CreateProductHandler : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken ct)
    {
        // ...
        return productId;
    }
}

// Register
services.AddMediator(typeof(CreateProductHandler).Assembly);
```

## Install

```bash
dotnet add package CodeYield.Mediator
```

## License

MIT
