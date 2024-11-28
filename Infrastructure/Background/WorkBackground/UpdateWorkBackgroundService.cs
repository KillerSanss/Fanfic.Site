using Application.Dto.WorkDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Background.WorkBackground;

/// <summary>
/// Фоновый сервис массового обновления Work
/// </summary>
public class UpdateWorkBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор
    /// </summary>
    public UpdateWorkBackgroundService(IServiceScopeFactory scopeFactory, IDistributedCache cache)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory);
        _cache = Guard.Against.Null(cache);
    }

    /// <summary>
    /// Массовое обновление Work
    /// </summary>
    /// <param name="stoppingToken">Токен остановки.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cacheWorksKey = "CACHED-WORKS-UPDATE";
            var cacheWorkTagsKey = "CACHED-WORKTAGS-UPDATE-CREATE";
            var cacheWorkTagsDeleteKey = "CACHED-WORKTAGS-DELETE";
            
            var cachedWorks = await _cache.GetStringAsync(cacheWorksKey, token: stoppingToken);
            var cachedWorkTags = await _cache.GetStringAsync(cacheWorkTagsKey, token: stoppingToken);
            var cachedWorkTagsDelete = await _cache.GetStringAsync(cacheWorkTagsDeleteKey, token: stoppingToken);
            
            if (!string.IsNullOrEmpty(cachedWorks))
            {
                var worksList = JsonConvert.DeserializeObject<List<UpdateWorkRequest>>(cachedWorks);
                var workTagsList = JsonConvert.DeserializeObject<List<WorkTag>>(cachedWorkTags);
                var workTagsDeleteList = JsonConvert.DeserializeObject<List<WorkTag>>(cachedWorkTagsDelete);
                var updatedWorks = new List<Work>();
                
                using var scope = _scopeFactory.CreateScope();
                var workRepository = scope.ServiceProvider.GetRequiredService<IWorkRepository>();
                var workTagRepository = scope.ServiceProvider.GetRequiredService<IWorkTagRepository>();
                
                foreach (var work in worksList)
                {
                    var updatedWork = await workRepository.GetByIdAsync(work.Id, stoppingToken);

                    updatedWork.Update(work.Title, work.Description, work.Category);
                    updatedWorks.Add(updatedWork);
                }
                
                await workRepository.BulkUpdateAsync(updatedWorks, stoppingToken);
                await workRepository.SaveChangesAsync(stoppingToken);
                await workTagRepository.BulkDeleteAsync(workTagsDeleteList, stoppingToken);
                await workTagRepository.BulkAddAsync(workTagsList, stoppingToken);
                await workTagRepository.SaveChangesAsync(stoppingToken);
                
                await _cache.RemoveAsync(cacheWorksKey, stoppingToken);
                await _cache.RemoveAsync(cacheWorkTagsKey, stoppingToken);
                await _cache.RemoveAsync(cacheWorkTagsDeleteKey, stoppingToken);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
        }
    }
}