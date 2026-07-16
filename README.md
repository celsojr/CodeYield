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
| **CodeYield.Mediator** | Lightweight CQRS contracts: commands, queries, results, pipeline behaviors, domain event handlers, and an in-process dispatcher. Pairs well with [MediatR](https://github.com/jbogard/MediatR) — see [Coexisting with MediatR](#coexisting-with-mediatr) |
| **CodeYield.Common** | Utilities: `Guard` clauses, `IClock`, `AsyncLazy<T>`, `RetryPolicy`, collection/string/enum extensions, `CyList<T>` |

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

### Pipeline Behaviors

```csharp
using CodeYield.Mediator;

// Auto-register all default behaviors (logging, performance, validation)
services.AddMediator(typeof(CreateProductHandler).Assembly);

// Or register behaviors individually
services.AddMediator(registerBehaviors: false, typeof(CreateProductHandler).Assembly);
services.AddLoggingBehavior();
services.AddPerformanceBehavior(thresholdMs: 300);
services.AddValidationBehavior();

// Custom behavior
public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken ct, Func<Task<TResponse>> next)
    {
        // Check authorization...
        return await next();
    }
}

services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
```

### Validation

```csharp
using CodeYield.Mediator;

public class CreateProductValidator : IValidator<CreateProductCommand>
{
    public ValueTask<ValidationResult> ValidateAsync(CreateProductCommand command, CancellationToken ct = default)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(command.Name))
            errors.Add("Name is required.");

        if (command.Price.Amount <= 0)
            errors.Add("Price must be greater than zero.");

        return ValueTask.FromResult(errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors.ToArray()));
    }
}

// Register the validator
services.AddTransient<IValidator<CreateProductCommand>, CreateProductValidator>();
```

### Coexisting with MediatR

[MediatR](https://github.com/jbogard/MediatR) is a mature, battle-tested library with a rich feature set (notifications, streaming, pre/post processors). CodeYield.Mediator is intentionally smaller — it provides just enough for the CQRS contracts used by the rest of this library. **They do not conflict**, and you can use both together.

The typical pattern is:

1. Keep MediatR as your primary dispatcher.
2. Implement CodeYield's marker interfaces (`ICommand<T>`, `IQuery<T>`) on your request records.
3. Write an adapter that bridges CodeYield's `IMediator` to MediatR's.

```csharp
using MediatR;
using CodeYield.Mediator;

// ---------------------------------------------------------------
// 1. Your commands carry both marker interfaces
// ---------------------------------------------------------------
public record CreateOrderCommand(Guid CustomerId, decimal Total)
    : ICommand<Guid>, IRequest<Guid>;

public record GetOrderQuery(Guid Id)
    : IQuery<OrderDto>, IRequest<OrderDto>;

// ---------------------------------------------------------------
// 2. Handlers target MediatR's contracts (or CodeYield's — your call)
// ---------------------------------------------------------------
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand cmd, CancellationToken ct)
    {
        // …persist the order…
        return Guid.NewGuid();
    }
}

// ---------------------------------------------------------------
// 3. Thin adapter: CodeYield's IMediator → MediatR's IMediator
// ---------------------------------------------------------------
public class MediatRMediatorAdapter : IMediator
{
    private readonly MediatR.IMediator _mediatR;

    public MediatRMediatorAdapter(MediatR.IMediator mediatR)
        => _mediatR = mediatR;

    public Task SendAsync(ICommand command, CancellationToken ct = default)
        => _mediatR.Send(new CommandEnvelope(command), ct);

    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
        => _mediatR.Send(new CommandEnvelope<TResponse>(command), ct);

    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
        => _mediatR.Send(new QueryEnvelope<TResponse>(query), ct);

    public Task PublishAsync<TEvent>(TEvent e, CancellationToken ct = default) where TEvent : IDomainEvent
        => _mediatR.Send(new DomainEventEnvelope<TEvent>(e), ct);
}

// ---------------------------------------------------------------
// 4. Envelope + handler wrappers (one per cardinality)
// ---------------------------------------------------------------
public record CommandEnvelope(ICommand Command) : IRequest;

public class CommandEnvelopeHandler(ICommandHandler handler)
    : IRequestHandler<CommandEnvelope>
{
    public async Task Handle(CommandEnvelope e, CancellationToken ct)
        => await handler.HandleAsync(e.Command, ct);
}

public record CommandEnvelope<T>(ICommand<T> Command) : IRequest<T>;

public class CommandEnvelopeHandler<T>(ICommandHandler<ICommand<T>, T> handler)
    : IRequestHandler<CommandEnvelope<T>, T>
{
    public async Task<T> Handle(CommandEnvelope<T> e, CancellationToken ct)
        => await handler.HandleAsync(e.Command, ct);
}

public record QueryEnvelope<T>(IQuery<T> Query) : IRequest<T>;

public class QueryEnvelopeHandler<T>(IQueryHandler<IQuery<T>, T> handler)
    : IRequestHandler<QueryEnvelope<T>, T>
{
    public async Task<T> Handle(QueryEnvelope<T> e, CancellationToken ct)
        => await handler.HandleAsync(e.Query, ct);
}

public record DomainEventEnvelope<T>(T Event) : IRequest where T : IDomainEvent;

// ---------------------------------------------------------------
// 5. Register everything
// ---------------------------------------------------------------
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// CodeYield handlers (optional — only if you also use ICommandHandler / IQueryHandler)
services.AddMediator(registerBehaviors: false, typeof(CreateOrderHandler).Assembly);

// Wire the adapter
services.AddSingleton<IMediator>(sp =>
    new MediatRMediatorAdapter(sp.GetRequiredService<MediatR.IMediator>()));
```

This gives you access to CodeYield's `Result<T>`, `IValidator<T>`, and pipeline behavior contracts while letting MediatR handle dispatching, notifications, and streaming.

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

### IClock

```csharp
using CodeYield.Common.Clock;

// Production: register the real clock
services.AddSingleton<IClock, SystemClock>();

// Usage in entities/services
public class Order(IClock clock)
{
    public DateTimeOffset CreatedAt { get; } = clock.UtcNow;
}

// Testing: freeze time
var clock = new FakeClock(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
```

### AsyncLazy

```csharp
using CodeYield.Common;

var config = new AsyncLazy<AppConfig>(async () =>
{
    return await ConfigService.LoadAsync();
});

// First access triggers the factory; subsequent accesses reuse the result
var cfg = await config.GetValueAsync();
```

### Retry

```csharp
using CodeYield.Common;

var result = await RetryPolicy.ExecuteAsync(
    async () => await httpClient.GetAsync("https://api.example.com/data"),
    maxRetries: 3,
    baseDelay: TimeSpan.FromMilliseconds(500));
```

### Collection Extensions

```csharp
using CodeYield.Common.Extensions;

var list = new List<int> { 1, 2, 3 };

list.AddIf(someCondition, 4);
list.AddIfNotNull(maybeItem);
list.AddRange(new[] { 5, 6, 7 });
list.RemoveAll(x => x % 2 == 0);   // removes evens

list.Shuffle();
var index = list.IndexOf(x => x == 3);

list.ForEach(x => Console.WriteLine(x));
```

### String Masking

```csharp
using CodeYield.Common.Extensions;

"john@example.com".MaskEmail();      // "j***@example.com"
"4111111111111111".MaskCreditCard(); // "4111-****-****-1111"
"555-867-5309".MaskPhone();          // "******5309"
"ABCDEFGH".MaskMiddle(2, 5);        // "AB***FGH"
```

### Enum Extensions

```csharp
using CodeYield.Common.Extensions;
using System.ComponentModel;

public enum Priority
{
    [Description("Low priority")]
    Low,
    [Description("High priority")]
    High
}

Priority.High.GetDescription(); // "High priority"
var all = EnumExtensions.GetValues<Priority>();
```

### Rich Iteration

```csharp
using CodeYield.Common.Collections;
using CodeYield.Common.Extensions;

var items = new CyList<string> { "Apple", "Banana", "Orange" };
Console.WriteLine(items); // ["Apple", "Banana", "Orange"]

// Works with any IEnumerable — prints nicely too
var numbers = new List<int> { 1, 2, 3 };
Console.WriteLine(numbers.AsLoop()); // [1, 2, 3]

// Rich iteration metadata
foreach (var loop in items.GetLoopContext())
{
    Console.WriteLine($"{loop.Index}: {loop.Item} (First: {loop.IsFirst}, Last: {loop.IsLast})");
}
```

## Requirements

- .NET 10.0+

## License

MIT
