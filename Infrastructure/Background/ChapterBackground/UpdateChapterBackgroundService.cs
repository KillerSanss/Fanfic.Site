using Application.Dto.ChapterDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.ChapterBackground;

/// <summary>
/// Фоновый сервис массового обновления Chapter
/// </summary>
public class UpdateChapterBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public UpdateChapterBackgroundService(
        IServiceScopeFactory scopeFactory,
        IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }
    
    /// <summary>
    /// Массовое обновление Chapter
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("       Фоновый сервис: Обновление глав");
            Console.WriteLine("======================================");
            
            var cacheChaptersKey = "CACHED-CHAPTERS-UPDATE";
            var cachedChapters = await _cache.GetStringAsync(cacheChaptersKey, token: stoppingToken);

            if (!string.IsNullOrEmpty(cachedChapters))
            {
                var chaptersList = JsonConvert.DeserializeObject<List<UpdateChapterRequest>>(cachedChapters);
                var updatedChapters = new List<Chapter>();

                using var scope = _scopeFactory.CreateScope();
                var chapterRepository = scope.ServiceProvider.GetRequiredService<IChapterRepository>();

                foreach (var chapter in chaptersList)
                {
                    var updatedChapter = await chapterRepository.GetByIdAsync(chapter.Id, stoppingToken);

                    updatedChapter.Update(chapter.Title, chapter.Description, chapter.Content);
                    updatedChapters.Add(updatedChapter);


                    await chapterRepository.BulkUpdateAsync(updatedChapters, stoppingToken);
                    await chapterRepository.SaveChangesAsync(stoppingToken);

                    await _cache.RemoveAsync(cacheChaptersKey, stoppingToken);
                }
            }
            
            await Task.Delay(TimeSpan.FromSeconds(65), stoppingToken);
        }
    }
}