using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.TagBackground.UserTagBackground;

/// <summary>
/// Фоновый сервис массовых действий UserTag
/// </summary>
public class UserTagBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public UserTagBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }
    
    /// <summary>
    /// Массовое действие UserTag
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        { 
            var cacheUserTagsKey = "CACHED-USERTAGS-CREATE";
            var cacheUserTagsDeleteKey = "CACHED-USERTAGS-DELETE";
            
            var cachedUserTags = await _cache.GetStringAsync(cacheUserTagsKey, token: stoppingToken);
            var cachedUserTagsDelete = await _cache.GetStringAsync(cacheUserTagsDeleteKey, token: stoppingToken);
    
            if (!string.IsNullOrEmpty(cachedUserTags))
            {
                var userTagsList = JsonConvert.DeserializeObject<List<UserTag>>(cachedUserTags);
                
                using var scope = _scopeFactory.CreateScope();
                var userTagRepository = scope.ServiceProvider.GetRequiredService<IUserTagRepository>();
                
                await userTagRepository.BulkAddAsync(userTagsList, stoppingToken);
                await userTagRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheUserTagsKey, stoppingToken);
            }
            
            if (!string.IsNullOrEmpty(cachedUserTagsDelete))
            {
                var userTagsDeleteList = JsonConvert.DeserializeObject<List<UserTag>>(cachedUserTagsDelete);
                
                using var scope = _scopeFactory.CreateScope();
                var userTagRepository = scope.ServiceProvider.GetRequiredService<IUserTagRepository>();
                
                await userTagRepository.BulkDeleteAsync(userTagsDeleteList, stoppingToken);
                await userTagRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheUserTagsDeleteKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }
}