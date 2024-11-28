using Application.Dto.CommentDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.CommentBackground;

/// <summary>
/// Фоновый сервис массового обновления Comment
/// </summary>
public class UpdateCommentBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public UpdateCommentBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }

    /// <summary>
    /// Массовое обновление Comment
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var commentRepository = scope.ServiceProvider.GetRequiredService<ICommentRepository>();
            
            var cacheCommentsKey = "CACHED-COMMENTS-UPDATE";
            var cachedComments = await _cache.GetStringAsync(cacheCommentsKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedComments))
            {
                var commentsList = JsonConvert.DeserializeObject<List<UpdateCommentRequest>>(cachedComments);
                var updatedComments = new List<Comment>();

                foreach (var comment in commentsList)
                {
                    var updatedComment = await commentRepository.GetByIdAsync(comment.Id, stoppingToken);

                    updatedComment.Update(comment.Content);
                    updatedComments.Add(updatedComment);
                }
                
                await commentRepository.BulkUpdateAsync(updatedComments, stoppingToken);
                await commentRepository.SaveChangesAsync(stoppingToken);

                await _cache.RemoveAsync(cacheCommentsKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(65), stoppingToken);
        }
    }
}