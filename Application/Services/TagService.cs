using Application.Dto.TagDto;
using Application.Dto.UserTagDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис Tag
/// </summary>
public class TagService
{
    private readonly ITagRepository _tagRepository;
    
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;
    private readonly UserTagService _userTagService;
    
    private readonly EmailService _emailService;
    private readonly EmailMessages _emailMessages;
    private readonly BotMessage _botMessage;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="emailService">Сервис Email.</param>
    /// <param name="emailMessages">EmailMessages.</param>
    /// <param name="tagRepository">Репозиторий Tag.</param>
    /// <param name="userTagService">Сервис UserTag.</param>
    /// <param name="mapper">Автомаппер.</param>
    /// <param name="botMessage">BotMessages.</param>
    /// <param name="addToCache">AddToCache.</param>
    public TagService(
        EmailService emailService,
        EmailMessages emailMessages,
        ITagRepository tagRepository,
        UserTagService userTagService,
        IMapper mapper,
        BotMessage botMessage,
        AddToCache addToCache)
    {
        _tagRepository = Guard.Against.Null(tagRepository);
        _userTagService = Guard.Against.Null(userTagService);
        
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
        
        _botMessage = Guard.Against.Null(botMessage);
        _emailService = Guard.Against.Null(emailService);
        _emailMessages = Guard.Against.Null(emailMessages);
    }
    
    /// <summary>
    /// Создание Tag
    /// </summary>
    /// <param name="tagRequest">Данные для Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый Tag.</returns>
    public async Task<CreateTagResponse> CreateAsync(
        CreateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(tagRequest);
        var tag = _mapper.Map<Tag>(tagRequest);
        
        await _addToCache.StoreInCache(tag, "CACHED-TAGS-CREATE", cancellationToken);
        
        return _mapper.Map<CreateTagResponse>(tag);
    }
    
    /// <summary>
    /// Обновление Tag
    /// </summary>
    /// <param name="tagRequest">Данные для обновления Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный Tag.</returns>
    public async Task<UpdateTagResponse> UpdateAsync(
        UpdateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(tagRequest);
        var tag = await GetByIdTag(tagRequest.Id, cancellationToken);
        
        await _addToCache.StoreInCache(tagRequest, "CACHED-TAGS-UPDATE", cancellationToken);

        return _mapper.Map<UpdateTagResponse>(tag);
    }

    /// <summary>
    /// Удаление Tag.
    /// </summary>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(Guid tagId, CancellationToken cancellationToken)
    {
        var tag = await GetByIdTag(tagId, cancellationToken);
        
        await _tagRepository.DeleteAsync(tag, cancellationToken);
        await _tagRepository.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Добавление Tag в избранное
    /// </summary>
    /// <param name="userTagRequest">Данные UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task FollowTagAsync(UserTagRequest userTagRequest, CancellationToken cancellationToken)
    {
        if (await _userTagService.HasUserLikedWorkAsync(userTagRequest.UserId, userTagRequest.TagId, cancellationToken))
            throw new InvalidOperationException("Пользователь уже добавил данный тэг в избранное.");
        
        await _userTagService.CreateAsync(userTagRequest, cancellationToken);
    }
    
    /// <summary>
    /// Удаление Tag из избранного
    /// </summary>
    /// <param name="userTagRequest">Данные UserTag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task UnfollowTagAsync(UserTagRequest userTagRequest, CancellationToken cancellationToken)
    {
        if (!await _userTagService.HasUserLikedWorkAsync(userTagRequest.UserId, userTagRequest.TagId, cancellationToken))
            throw new InvalidOperationException("Пользователь не добавил данный тэг в избранное.");
        
        await _userTagService.DeleteAsync(userTagRequest, cancellationToken);
    }
    
    /// <summary>
    /// Получение Tag по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Tag.</returns>
    public async Task<GetTagResponse> GetByIdAsync(Guid tagId, CancellationToken cancellationToken)
    {
        var tag = await GetByIdTag(tagId, cancellationToken);
        return _mapper.Map<GetTagResponse>(tag);
    }
    
    /// <summary>
    /// Получение всех Tag
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Tag.</returns>
    public async Task<List<GetTagResponse>> GetAllTagAsync(CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<GetTagResponse>>(tags);
    }
    
    private async Task<Tag> GetByIdTag(Guid id, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(id, cancellationToken);
        if (tag == null)
            return null;

        return tag;
    }
}