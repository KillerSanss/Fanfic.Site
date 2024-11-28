using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application;

/// <summary>
/// Класс добавления в кэш
/// </summary>
public class AddToCache
{
    private readonly IDistributedCache _cache;

    public AddToCache(
        IDistributedCache cache)
    {
        _cache = cache;
    }
    
    /// <summary>
    /// Добавление в кэш
    /// </summary>
    /// <param name="item">Сущность для добавления.</param>
    /// <param name="cacheKey">Ключ кэша.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public async Task StoreInCache<T>(T item, string cacheKey, CancellationToken cancellationToken)
    {
        var existingItems = await _cache.GetStringAsync(cacheKey, token: cancellationToken);
        var itemsList = string.IsNullOrEmpty(existingItems) ? [] : JsonConvert.DeserializeObject<List<T>>(existingItems);

        itemsList.Add(item);
        
        var serializedItems = JsonConvert.SerializeObject(itemsList);
        var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(65) };
        
        await _cache.SetStringAsync(cacheKey, serializedItems, cacheOptions, token: cancellationToken);
    }
    
    /// <summary>
    /// Добавление в кэш
    /// </summary>
    /// <param name="items">Список для добавления.</param>
    /// <param name="cacheKey">Ключ кэша.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public async Task StoreListInCache<T>(List<T> items, string cacheKey, CancellationToken cancellationToken)
    {
        var serializedItems = JsonConvert.SerializeObject(items);
        var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(65) };
        
        await _cache.SetStringAsync(cacheKey, serializedItems, cacheOptions, token: cancellationToken);
    }
}