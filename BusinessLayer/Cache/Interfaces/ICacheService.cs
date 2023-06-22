namespace BusinessLayer.Cache.Interfaces;

/// <summary>
/// Caching service interface
/// </summary>
public interface ICacheService
{
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;

    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
       where T : class;

    Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
       where T : class;
}
