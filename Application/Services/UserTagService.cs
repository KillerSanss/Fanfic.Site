using Application.Dto.UserTagDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserTagService
{
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;

    private readonly IUserTagRepository _userTagRepository;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="mapper">Автомаппер</param>
    /// <param name="addToCache">AddToCache.</param>
    /// <param name="userTagRepository">Репозиторий UserTag.</param>
    public UserTagService(
        IMapper mapper,
        AddToCache addToCache,
        IUserTagRepository userTagRepository)
    {
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
        _userTagRepository = Guard.Against.Null(userTagRepository);
    }
    
    /// <summary>
    /// Создание UserTag
    /// </summary>
    /// <param name="userTagRequest">UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый UserTag.</returns>
    public async Task CreateAsync(
        UserTagRequest userTagRequest,
        CancellationToken cancellationToken)
    {
        var userTag = _mapper.Map<UserTag>(userTagRequest);
        
        await _addToCache.StoreInCache(userTag, "CACHED-USERTAGS-CREATE", cancellationToken);
    }
    
    /// <summary>
    /// Удаление UserTag
    /// </summary>
    /// <param name="userTagRequest">UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(
        UserTagRequest userTagRequest,
        CancellationToken cancellationToken)
    {
        var userTag = _mapper.Map<UserTag>(userTagRequest);
        await _addToCache.StoreInCache(userTag, "CACHED-USERTAGS-DELETE", cancellationToken);
    }

    /// <summary>
    /// Проверка наличия любимого тэга у User
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task<bool> HasUserLikedWorkAsync(
        Guid userId,
        Guid tagId,
        CancellationToken cancellationToken)
    {
        return await _userTagRepository.HasUserLikedTagAsync(userId, tagId, cancellationToken);
    }
}