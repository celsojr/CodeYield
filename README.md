# CodeYield

Reusable Domain-Driven Design building blocks for .NET applications.

## Projects

| Package | Description |
|---|---|
| **CodeYield.Abstractions** | Core interfaces: `IEntity<TId>`, `IAggregateRoot`, `IDomainEvent`, `IEditableEntity`, `ITenantContext` |
| **CodeYield.Domain** | Base classes: `BaseEntity<TId>`, `BaseAggregateRoot<TId>`, `AuditableEntity<TId>`, `ValueObject`, `ValueObject<T>`, `Enumeration<TEnum>` |
| **CodeYield.Persistence** | Repository abstraction (`IRepository<T, TId>`), `PaginatedResult<T>`, and composable `Specification<T>` |
| **CodeYield.Exceptions** | Typed exception hierarchy: `DomainException`, `NotFoundException`, `ValidationException` |
| **CodeYield.EventBus** | Channel-based in-process event bus with HTTP delivery and automatic retry |
| **CodeYield.Mediator** | Lightweight CQRS abstractions: commands, queries, results, domain event handlers, and an in-process dispatcher |
| **CodeYield.Common** | Utilities: `Guard` clauses, `CyList<T>` with rich iteration metadata |

## Getting Started

Reference the packages you need in your `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="path\to\CodeYield.Domain" />
  <ProjectReference Include="path\to\CodeYield.Mediator" />
</ItemGroup>
```

## Usage

### Defining Entities

```csharp
using CodeYield.Domain;

public class Product : BaseAggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;

    private Product() { }

    public static Product Create(string name, Money price)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };
    }
}
```

### Value Objects

```csharp
using CodeYield.Domain;

// Built-in Email value object
var email = Email.Create("user@example.com");

// Built-in Money value object
var price = Money.Create(29.99m, "USD");
var total = price.Add(Money.Create(5.00m, "USD"));

// Custom multi-component value object
public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
    }
}
```

### Smart Enums

```csharp
using CodeYield.Domain;

public class OrderStatus : Enumeration<OrderStatus>
{
    public static readonly OrderStatus Pending = new(1, nameof(Pending));
    public static readonly OrderStatus Confirmed = new(2, nameof(Confirmed));
    public static readonly OrderStatus Shipped = new(3, nameof(Shipped));

    protected OrderStatus(int value, string name) : base(value, name) { }

    public bool CanTransitionTo(OrderStatus next) =>
        (this == Pending && next == Confirmed) ||
        (this == Confirmed && next == Shipped);
}

// Lookup by value or name
var status = OrderStatus.FromValue(1);
var all = OrderStatus.GetAll();
```

### Guard Clauses

```csharp
using CodeYield.Common;

public Product Create(string name, decimal price)
{
    Guard.AgainstNullOrEmpty(name, nameof(name));
    Guard.AgainstNegativeOrZero(price, nameof(price));
    // ...
}
```

### Specifications

```csharp
using CodeYield.Persistence;

public class ActiveProducts : Specification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression() =>
        product => product.IsActive;

    public static readonly ActiveProducts Instance = new();
}

// Compose with And/Or/Not
var spec = new ActiveProducts()
    .And(new ProductsByCategory("Electronics"))
    .Not(new DiscontinuedProducts());

var results = await repository.GetAllAsync(spec, page: 1, pageSize: 10);
```

### CQRS with Mediator

```csharp
using CodeYield.Mediator;

// Define a command
public record CreateProductCommand(string Name, Money Price) : ICommand<Guid>;

// Handle it
public class CreateProductHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IRepository<Product, Guid> _repository;

    public CreateProductHandler(IRepository<Product, Guid> repository) => _repository = repository;

    public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken ct)
    {
        var product = Product.Create(command.Name, command.Price);
        await _repository.AddAsync(product, ct);
        return product.Id;
    }
}

// Register and dispatch
services.AddMediator(typeof(CreateProductHandler).Assembly);

var productId = await _mediator.SendAsync(new CreateProductCommand("Widget", Money.Create(9.99m, "USD")));
```

### Results

```csharp
using CodeYield.Mediator;

public Result<Order> PlaceOrder(Cart cart)
{
    if (cart.Items.Count == 0)
        return Result<Order>.Failure("Cart is empty");

    var order = Order.From(cart);
    return Result<Order>.Success(order);
}

public Result DeleteOrder(Guid id)
{
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

Failed deliveries are retried with exponential backoff (configurable via `MaxRetries` and `RetryDelayMs`).

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
