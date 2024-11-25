using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.WorkBackground.WorkLikeBackground;

/// <summary>
/// Фоновый сервис массовых действий WorkLike
/// </summary>
public class WorkLikeBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public WorkLikeBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }
    
    /// <summary>
    /// Массовое действие WorkLike
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        { 
            Console.WriteLine("==================================================");
            Console.WriteLine("       Фоновый сервис: Добавление/удаление лайков");
            Console.WriteLine("==================================================");
        
            var cacheWorkLikesKey = "CACHED-WORKLIKES-CREATE";
            var cacheWorkLikesDeleteKey = "CACHED-WORKLIKES-DELETE";
            
            var cachedWorkLikes = await _cache.GetStringAsync(cacheWorkLikesKey, token: stoppingToken);
            var cachedWorkLikesDelete = await _cache.GetStringAsync(cacheWorkLikesDeleteKey, token: stoppingToken);
    
            if (!string.IsNullOrEmpty(cachedWorkLikes))
            {
                var workLikesList = JsonConvert.DeserializeObject<List<WorkLike>>(cachedWorkLikes);
                
                using var scope = _scopeFactory.CreateScope();
                var workLikeRepository = scope.ServiceProvider.GetRequiredService<IWorkLikeRepository>();
                
                await workLikeRepository.BulkAddAsync(workLikesList, stoppingToken);
                await workLikeRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheWorkLikesKey, stoppingToken);
            }
            
            if (!string.IsNullOrEmpty(cachedWorkLikesDelete))
            {
                var workLikesDeleteList = JsonConvert.DeserializeObject<List<WorkLike>>(cachedWorkLikesDelete);
                
                using var scope = _scopeFactory.CreateScope();
                var workLikeRepository = scope.ServiceProvider.GetRequiredService<IWorkLikeRepository>();
                
                await workLikeRepository.BulkDeleteAsync(workLikesDeleteList, stoppingToken);
                await workLikeRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheWorkLikesDeleteKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }
}