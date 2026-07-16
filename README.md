# CodeYield

Reusable Domain-Driven Design building blocks for .NET applications.

## Projects

| Package | Description |
|---|---|
| **CodeYield.Abstractions** | Core interfaces: `IEntity<TId>`, `IAggregateRoot`, `IDomainEvent`, `IEditableEntity`, `ITenantContext` |
| **CodeYield.Domain** | Base classes implementing the abstractions: `BaseEntity<TId>`, `BaseAggregateRoot<TId>`, `AuditableEntity<TId>`, `ValueObject`, `ValueObject<T>` |
| **CodeYield.Persistence** | Repository abstraction (`IRepository<T, TId>`) and `PaginatedResult<T>` |
| **CodeYield.Results** | Standardized result wrappers: `Result`, `Result<T>` |
| **CodeYield.Exceptions** | Typed exception hierarchy: `DomainException`, `NotFoundException`, `ValidationException` |
| **CodeYield.EventBus** | In-process event bus with channel-based pub/sub and HTTP delivery with automatic retry |
| **CodeYield.Common** | Utility collection: `CyList<T>` with rich iteration metadata via `LoopContext<T>` |

## Getting Started

Reference the packages you need in your `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="path\to\CodeYield.Domain" />
  <ProjectReference Include="path\to\CodeYield.Results" />
</ItemGroup>
```

## Usage

### Defining Entities

```csharp
using CodeYield.Domain;

public class Product : BaseAggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    private Product() { }

    public static Product Create(string name, decimal price)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };
        return product;
    }
}
```

### Value Objects

```csharp
using CodeYield.Domain;

// Single-component value object
public class Email : ValueObject<Email>
{
    public string Value { get; }

    protected override Email EqualityValue => this;

    private Email(string value) => Value = value;

    public static Email Create(string value) => new(value);

    public override bool Equals(object? obj) =>
        obj is Email other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}

// Multi-component value object
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

### Results

```csharp
using CodeYield.Results;

public Result<Order> PlaceOrder(Cart cart)
{
    if (cart.Items.Count == 0)
        return Result<Order>.Failure("Cart is empty");

    var order = Order.From(cart);
    return Result<Order>.Success(order);
}

public Result DeleteOrder(Guid id)
{
    // ... delete logic
    return Result.Success();
}
```

### Event Bus

```csharp
using CodeYield.EventBus;

// Register services
services.AddEventBus();

// Configure subscribers
services.Configure<EventBusOptions>(options =>
{
    options.Subscribers["OrderCreated"] = new List<SubscriberConfig>
    {
        new() { Url = "https://notifications.example.com/events/order-created" }
    };
});

// Publish events
await _eventBus.PublishAsync(new OrderCreated(order.Id, order.Total));
```

Failed deliveries are retried automatically with exponential backoff (configurable via `MaxRetries` and `RetryDelayMs` on `SubscriberConfig`).

### Rich Iteration

```csharp
using CodeYield.Common.Collections;
using CodeYield.Common.Extensions;

var items = new CyList<string> { "Apple", "Banana", "Orange" };
foreach (var loop in items.GetLoopContext())
{
    Console.WriteLine($"{loop.Index}: {loop.Item} (First: {loop.IsFirst}, Last: {loop.IsLast})");
}

// Works with any IEnumerable
var numbers = new List<int> { 1, 2, 3 };
foreach (var loop in numbers.AsLoop())
{
    Console.WriteLine($"{loop.Step}: {loop.Item}");
}
```

## Requirements

- .NET 10.0+

## License

MIT
