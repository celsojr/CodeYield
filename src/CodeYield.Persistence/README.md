# CodeYield.Persistence

Repository abstraction, pagination, and composable specifications for .NET.

[![NuGet](https://img.shields.io/nuget/v/CodeYield.Persistence.svg)](https://www.nuget.org/packages/CodeYield.Persistence)

## Features

### Repository

```csharp
public interface IRepository<T, TId> where T : IEntity<TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> GetAllAsync(Specification<T>? spec = null, int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
    Task<int> CountAsync(Specification<T>? spec = null, CancellationToken ct = default);
}
```

### Specifications

Composable query building blocks:

```csharp
var spec = new ActiveProducts()
    .And(new ProductsByCategory("Electronics"))
    .Not(new DiscontinuedProducts());

var results = await repository.GetAllAsync(spec, page: 1, pageSize: 10);
```

### PaginatedResult

```csharp
public record PaginatedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
```

## Install

```bash
dotnet add package CodeYield.Persistence
```

## License

MIT
