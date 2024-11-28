using Application.Dto.CommentDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис Comment
/// </summary>
public class CommentService
{
    private readonly ICommentRepository _commentRepository;
    
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="commentRepository">Репозиторий Comment.</param>
    /// <param name="mapper">Автомаппер.</param>
    /// <param name="addToCache">AddToCache.</param>
    public CommentService(
        ICommentRepository commentRepository,
        IMapper mapper,
        AddToCache addToCache)
    {
        _commentRepository = Guard.Against.Null(commentRepository);
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
    }
    
    /// <summary>
    /// Создание Comment
    /// </summary>
    /// <param name="commentRequest">Данные для Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый Comment.</returns>
    public async Task<CreateCommentResponse> CreateAsync(
        CreateCommentRequest commentRequest,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(commentRequest);
        var comment = _mapper.Map<Comment>(commentRequest);
        
        await _addToCache.StoreInCache(comment, "CACHED-COMMENTS-CREATE", cancellationToken);
        
        return _mapper.Map<CreateCommentResponse>(comment);
    }
    
    /// <summary>
    /// Обновление Comment
    /// </summary>
    /// <param name="commentRequest">Данные для обновления Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный Comment.</returns>
    public async Task<UpdateCommentResponse> UpdateAsync(
        UpdateCommentRequest commentRequest,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(commentRequest);
        var comment = await GetByIdComment(commentRequest.Id, cancellationToken);
        
        await _addToCache.StoreInCache(commentRequest, "CACHED-COMMENTS-UPDATE", cancellationToken);

        return _mapper.Map<UpdateCommentResponse>(comment);
    }

    /// <summary>
    /// Удаление Comment.
    /// </summary>
    /// <param name="commentId">Идентификатор Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await GetByIdComment(commentId, cancellationToken);
        
        try
        {
            await _commentRepository.DeleteAsync(comment, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        await _commentRepository.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получение Comment по идентификатору
    /// </summary>
    /// <param name="commentId">Идентификатор Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Comment.</returns>
    public async Task<GetCommentResponse> GetByIdAsync(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await GetByIdComment(commentId, cancellationToken);
        return _mapper.Map<GetCommentResponse>(comment);
    }
    
    /// <summary>
    /// Получение всех Comment
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public async Task<List<GetCommentResponse>> GetAllCommentAsync(CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<GetCommentResponse>>(comments);
    }

    /// <summary>
    /// Получение всех Comment у главы
    /// </summary>
    /// <param name="chapterId">Идентификатор Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    public async Task<List<GetCommentResponse>> GetAllChapterCommentAsync(
        Guid chapterId,
        CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetAllChapterCommentsAsync(chapterId, cancellationToken);
        return _mapper.Map<List<GetCommentResponse>>(comments);
    }
    
    private async Task<Comment> GetByIdComment(Guid id, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(id, cancellationToken);
        if (comment == null)
            return null;

        return comment;
    }
}