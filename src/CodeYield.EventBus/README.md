# CodeYield.EventBus

Channel-based in-process event bus with HTTP delivery and automatic retry for .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.EventBus.svg)](https://www.nuget.org/packages/CodeYield.EventBus)

## Features

- In-process pub/sub using `System.Threading.Channels`
- HTTP delivery to configured subscriber URLs
- Automatic retry with exponential backoff
- Configurable via `IOptions<EventBusOptions>`

## Usage

```csharp
// Register
services.AddEventBus();

// Configure subscribers
services.Configure<EventBusOptions>(options =>
{
    options.Subscribers["OrderCreated"] = new List<SubscriberConfig>
    {
        new() { Url = "https://notifications.example.com/events/order-created" }
    };
});

// Publish
await _eventBus.PublishAsync(new OrderCreated(order.Id, order.Total));
```

## Install

```bash
dotnet add package CodeYield.EventBus
```

## License

MIT
