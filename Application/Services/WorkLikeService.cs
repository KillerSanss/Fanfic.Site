using Application.Dto.WorkLikeDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис WorkLike
/// </summary>
public class WorkLikeService
{
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;

    private readonly IWorkLikeRepository _workLikeRepository;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="mapper">Автомаппер</param>
    /// <param name="addToCache">AddToCache.</param>
    /// <param name="workLikeRepository">Репозиторий WorkLike.</param>
    public WorkLikeService(
        IMapper mapper,
        AddToCache addToCache,
        IWorkLikeRepository workLikeRepository)
    {
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
        _workLikeRepository = Guard.Against.Null(workLikeRepository);
    }
    
    /// <summary>
    /// Создание WorkLike
    /// </summary>
    /// <param name="workLikeRequest">WorkLike.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый WorkLike.</returns>
    public async Task CreateAsync(
        WorkLikeRequest workLikeRequest,
        CancellationToken cancellationToken)
    {
        var workLike = _mapper.Map<WorkLike>(workLikeRequest);
        
        await _addToCache.StoreInCache(workLike, "CACHED-WORKLIKES-CREATE", cancellationToken);
    }
    
    /// <summary>
    /// Удаление WorkLike
    /// </summary>
    /// <param name="workLikeRequest">WorkLike.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(
        WorkLikeRequest workLikeRequest,
        CancellationToken cancellationToken)
    {
        var workLike = _mapper.Map<WorkLike>(workLikeRequest);
        await _addToCache.StoreInCache(workLike, "CACHED-WORKLIKES-DELETE", cancellationToken);
    }

    /// <summary>
    /// Проверка наличия лайка от User
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<bool> HasUserLikedWorkAsync(
        Guid userId,
        Guid workId,
        CancellationToken cancellationToken)
    {
        return await _workLikeRepository.HasUserLikedWorkAsync(userId, workId, cancellationToken);
    }
}