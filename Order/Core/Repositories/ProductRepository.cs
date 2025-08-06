using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Order.Core.Domain.Entities;
using Order.Core.Repositories.Interfaces;
using StackExchange.Redis;

namespace Order.Core.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;

    public ProductRepository(IDistributedCache cache, IConnectionMultiplexer redis)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
    }
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var serialized = await _cache.GetStringAsync(id.ToString());
        if (string.IsNullOrEmpty(serialized))
        {
            return null;
        }

        var product = JsonSerializer.Deserialize<Product>(serialized);

        if (product == null)
        {
            return null;
        }
        return product!;
    }
}