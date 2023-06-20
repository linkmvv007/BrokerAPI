using BusinessLayer.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BusinessLayer.Cache;

/// <summary>
/// Caching service
/// </summary>
public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    /// <summary>
    /// Initialize a new instance of <see cref="CacheService"/>
    /// </summary>
    /// <param name="distributedCache">Represents a distributed cache of serialized values.</param>
    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    /// <summary>
    ///  Write object to cache
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    /// <param name="key">cache key</param>
    /// <param name="value">object that is cached</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken)
        where T : class
    {
        await _distributedCache.SetStringAsync(
            key,
            JsonSerializer.Serialize(value),
            Consts.GetDistributedCacheEntryOptions,
            cancellationToken);
    }

    /// <summary>
    /// Read object from cache
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    /// <param name="key">cache key</param>
    /// <param name="cancellationToken"></param>
    /// <returns>object from cache</returns>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        where T : class
    {
        var cacheData = await _distributedCache.GetAsync(key, cancellationToken);

        return cacheData is null ? null : JsonSerializer.Deserialize<T>(cacheData);
    }

    /// <summary>
    /// Reads an object using the cache.
    /// If the object is not in the cache, then gets the object using the function <see cref="factory"/> and then puts it in the cache
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    /// <param name="key">cache key</param>
    /// <param name="factory">Function that gets the object if it's not in the cache</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken)
        where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        if (cachedValue is not null)
        {
            await SetAsync(key, cachedValue, cancellationToken);
        }

        return cachedValue;
    }
}

