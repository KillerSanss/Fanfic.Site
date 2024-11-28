using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.WorkBackground;

/// <summary>
/// Фоновый сервис массового создания Work
/// </summary>
public class CreateWorkBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public CreateWorkBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }

    /// <summary>
    /// Массовое создание Work
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cacheWorksKey = "CACHED-WORKS-CREATE";
            var cacheWorkTagsKey = "CACHED-WORKTAGS-CREATE";
            
            var cachedWorks = await _cache.GetStringAsync(cacheWorksKey, token: stoppingToken);
            var cachedWorkTags = await _cache.GetStringAsync(cacheWorkTagsKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedWorks))
            {
                var worksList = JsonConvert.DeserializeObject<List<Work>>(cachedWorks);
                var workTagsList = JsonConvert.DeserializeObject<List<WorkTag>>(cachedWorkTags);
            
                using var scope = _scopeFactory.CreateScope();
                var workRepository = scope.ServiceProvider.GetRequiredService<IWorkRepository>();
                var workTagRepository = scope.ServiceProvider.GetRequiredService<IWorkTagRepository>();
                    
                await workRepository.BulkAddAsync(worksList, stoppingToken);
                await workRepository.SaveChangesAsync(stoppingToken);
                await workTagRepository.BulkAddAsync(workTagsList, stoppingToken);
                await workTagRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheWorksKey, stoppingToken);
                await _cache.RemoveAsync(cacheWorkTagsKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }
    }
}