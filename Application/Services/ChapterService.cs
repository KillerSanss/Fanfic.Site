using Application.Dto.ChapterDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис Chapter
/// </summary>
public class ChapterService
{
    private readonly IChapterRepository _chapterRepository;
    
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="chapterRepository">Репозиторий Chapter.</param>
    /// <param name="mapper">Автомаппер.</param>
    /// <param name="addToCache">AddToCache.</param>
    public ChapterService(
        IChapterRepository chapterRepository,
        IMapper mapper,
        AddToCache addToCache)
    {
        _chapterRepository = Guard.Against.Null(chapterRepository);
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
    }

    /// <summary>
    /// Создание Chapter
    /// </summary>
    /// <param name="chapterRequest">Данные для Chapter.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новая Work.</returns>
    public async Task<CreateChapterResponse> CreateAsync(
        CreateChapterRequest chapterRequest,
        Guid workId,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(chapterRequest);
        var chapter = _mapper.Map<Chapter>(chapterRequest);
        chapter.WorkId = workId;
        
        await _addToCache.StoreInCache(chapter, "CACHED-CHAPTERS-CREATE", cancellationToken);
        
        return _mapper.Map<CreateChapterResponse>(chapter);
    }
    
    /// <summary>
    /// Обновление Chapter
    /// </summary>
    /// <param name="chapterRequest">Данные для обновления Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный Chapter.</returns>
    public async Task<UpdateChapterResponse> UpdateAsync(
        UpdateChapterRequest chapterRequest,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(chapterRequest);
        var chapter = await GetByIdChapter(chapterRequest.Id, cancellationToken);
        
        await _addToCache.StoreInCache(chapterRequest, "CACHED-CHAPTERS-UPDATE", cancellationToken);

        return _mapper.Map<UpdateChapterResponse>(chapter);
    }

    /// <summary>
    /// Удаление Chapter.
    /// </summary>
    /// <param name="chapterId">Идентификатор Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(Guid chapterId, CancellationToken cancellationToken)
    {
        var chapter = await GetByIdChapter(chapterId, cancellationToken);
        
        await _chapterRepository.DeleteAsync(chapter, cancellationToken);
        await _chapterRepository.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Получение Chapter по идентификатору
    /// </summary>
    /// <param name="chapterId">Идентификатор Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Chapter.</returns>
    public async Task<GetChapterResponse> GetByIdAsync(Guid chapterId, CancellationToken cancellationToken)
    {
        var chapter = await GetByIdChapter(chapterId, cancellationToken);
        return _mapper.Map<GetChapterResponse>(chapter);
    }
    
    /// <summary>
    /// Получение всех Chapter
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    public async Task<List<GetChapterResponse>> GetAllChapterAsync(CancellationToken cancellationToken)
    {
        var chapters = await _chapterRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<GetChapterResponse>>(chapters);
    }

    /// <summary>
    /// Получение всех Chapter работы
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    public async Task<List<GetChapterResponse>> GetAllWorkChapterAsync(Guid workId, CancellationToken cancellationToken)
    {
        var chapters = await _chapterRepository.GetAllWorkChaptersAsync(workId, cancellationToken);
        return _mapper.Map<List<GetChapterResponse>>(chapters);
    }
    
    private async Task<Chapter> GetByIdChapter(Guid id, CancellationToken cancellationToken)
    {
        var chapter = await _chapterRepository.GetByIdAsync(id, cancellationToken);
        if (chapter == null)
            return null;

        return chapter;
    }
}