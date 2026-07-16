# CodeYield.Common

Utilities, guard clauses, extensions, and helpers for .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.Common.svg)](https://www.nuget.org/packages/CodeYield.Common)

## Features

### Guard Clauses

```csharp
Guard.AgainstNullOrEmpty(name, nameof(name));
Guard.AgainstNegativeOrZero(price, nameof(price));
```

### IClock

```csharp
services.AddSingleton<IClock, SystemClock>();      // production
var clock = new FakeClock(someFixedDate);            // testing
```

### AsyncLazy\<T\>

```csharp
var config = new AsyncLazy<AppConfig>(async () => await ConfigService.LoadAsync());
var cfg = await config.GetValueAsync();
```

### RetryPolicy

```csharp
await RetryPolicy.ExecuteAsync(
    async () => await httpClient.GetAsync(url),
    maxRetries: 3,
    baseDelay: TimeSpan.FromMilliseconds(500));
```

### Extensions

- **Collections** — `AddIf`, `AddIfNotNull`, `Shuffle`, `ForEach`, `IndexOf`
- **Strings** — `MaskEmail`, `MaskCreditCard`, `MaskPhone`, `MaskMiddle`
- **Enums** — `GetDescription`, `GetValues<T>`
- **CyList\<T\>** — rich iteration with `GetLoopContext()` for first/last/index metadata

## Install

```bash
dotnet add package CodeYield.Common
```

## License

MIT
