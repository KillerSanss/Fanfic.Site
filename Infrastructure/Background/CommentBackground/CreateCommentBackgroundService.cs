using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.CommentBackground;

/// <summary>
/// Фоновый сервис массового создания Comment
/// </summary>
public class CreateCommentBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public CreateCommentBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }

    /// <summary>
    /// Массовое создание Comment
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cacheCommentsKey = "CACHED-COMMENTS-CREATE";
            
            var cachedComments = await _cache.GetStringAsync(cacheCommentsKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedComments))
            {
                var commentsList = JsonConvert.DeserializeObject<List<Comment>>(cachedComments);
            
                using var scope = _scopeFactory.CreateScope();
                var commentRepository = scope.ServiceProvider.GetRequiredService<ICommentRepository>();
                    
                await commentRepository.BulkAddAsync(commentsList, stoppingToken);
                await commentRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheCommentsKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(65), stoppingToken);
        }
    }
}