using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.ChapterBackground;

/// <summary>
/// Фоновый сервис массового создания Chapter
/// </summary>
public class CreateChapterBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public CreateChapterBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }
    
    /// <summary>
    /// Массовое создание Chapter
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine("       Фоновый сервис: Добавление глав");
            Console.WriteLine("=======================================");
            
            var cacheChaptersKey = "CACHED-CHAPTERS-CREATE";
            
            var cachedChapters = await _cache.GetStringAsync(cacheChaptersKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedChapters))
            {
                var chaptersList = JsonConvert.DeserializeObject<List<Chapter>>(cachedChapters);
            
                using var scope = _scopeFactory.CreateScope();
                var chapterRepository = scope.ServiceProvider.GetRequiredService<IChapterRepository>();
                    
                await chapterRepository.BulkAddAsync(chaptersList, stoppingToken);
                await chapterRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheChaptersKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(65), stoppingToken);
        }
    }
}