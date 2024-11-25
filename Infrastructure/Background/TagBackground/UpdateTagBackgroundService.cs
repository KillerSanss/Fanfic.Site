using Application.Dto.TagDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.TagBackground;

/// <summary>
/// Фоновый сервис массового обновления Tag
/// </summary>
public class UpdateTagBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    public UpdateTagBackgroundService(IServiceScopeFactory scopeFactory, IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }

    /// <summary>
    /// Массовое обновление Tag
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("       Фоновый сервис: Обновление тэгов");
            Console.WriteLine("=======================================");
            
            var cacheKey = "CACHED-TAGS-UPDATE";
            var cachedTags = await _cache.GetStringAsync(cacheKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedTags))
            {
                var tagsList = JsonConvert.DeserializeObject<List<UpdateTagRequest>>(cachedTags);
                var updatedTags = new List<Tag>();
                
                using var scope = _scopeFactory.CreateScope();
                var tagRepository = scope.ServiceProvider.GetRequiredService<ITagRepository>();
                
                foreach (var tag in tagsList)
                {
                    var updatedTag = await tagRepository.GetByIdAsync(tag.Id, stoppingToken);

                    updatedTag.Update(tag.Name, tag.AgeRestriction, tag.Description);
                    updatedTags.Add(updatedTag);
                }
                
                await tagRepository.BulkUpdateAsync(updatedTags, stoppingToken);
                await tagRepository.SaveChangesAsync(stoppingToken);
                await _cache.RemoveAsync(cacheKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }
}