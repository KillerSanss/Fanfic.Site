using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.TagBackground;

/// <summary>
/// Фоновый сервис массового создания Tag
/// </summary>
public class CreateTagBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public CreateTagBackgroundService(IServiceScopeFactory scopeFactory, IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }

    /// <summary>
    /// Массовое создание Tag
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cacheKey = "CACHED-TAGS-CREATE";
            var cachedTags = await _cache.GetStringAsync(cacheKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedTags))
            {
                var tagsList = JsonConvert.DeserializeObject<List<Tag>>(cachedTags);
            
                using var scope = _scopeFactory.CreateScope();
                var tagRepository = scope.ServiceProvider.GetRequiredService<ITagRepository>();

                await tagRepository.BulkAddAsync(tagsList, stoppingToken);
                await tagRepository.SaveChangesAsync(stoppingToken);
                await _cache.RemoveAsync(cacheKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}