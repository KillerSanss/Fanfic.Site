using Application.Dto.WorkTagDto;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис WorkTag
/// </summary>
public class WorkTagService
{
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="mapper">Автомаппер</param>
    /// <param name="addToCache">AddToCache.</param>
    public WorkTagService(
        IMapper mapper,
        AddToCache addToCache)
    {
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
    }
    
    /// <summary>
    /// Создание WorkTag
    /// </summary>
    /// <param name="workTagRequests">Список WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый WorkTag.</returns>
    public async Task CreateAsync(
        List<WorkTagRequest> workTagRequests,
        CancellationToken cancellationToken)
    {
        var workTags = _mapper.Map<List<WorkTag>>(workTagRequests);
        
        await _addToCache.StoreListInCache(workTags, "CACHED-WORKTAGS-CREATE", cancellationToken);
    }
    
    /// <summary>
    /// Обновление WorkTag
    /// </summary>
    /// <param name="workTagRequests">Список WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый WorkTag.</returns>
    public async Task UpdateAsync(
        List<WorkTagRequest> workTagRequests,
        CancellationToken cancellationToken)
    {
        var workTags = _mapper.Map<List<WorkTag>>(workTagRequests);
        
        await _addToCache.StoreListInCache(workTags, "CACHED-WORKTAGS-UPDATE-CREATE", cancellationToken);
    }
    
    /// <summary>
    /// Удаление WorkTag
    /// </summary>
    /// <param name="workTagRequests">Список WorkTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(
        List<WorkTagRequest> workTagRequests,
        CancellationToken cancellationToken)
    {
        var workTags = _mapper.Map<List<WorkTag>>(workTagRequests);
        await _addToCache.StoreListInCache(workTags, "CACHED-WORKTAGS-DELETE", cancellationToken);
    }
}