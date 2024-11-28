using Application.Dto.WorkDto;
using Application.Dto.WorkLikeDto;
using Application.Dto.WorkTagDto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис Work
/// </summary>
public class WorkService
{
    private readonly IWorkRepository _workRepository;
    
    private readonly WorkTagService _workTagService;
    private readonly WorkLikeService _workLikeService;
    private readonly ChapterService _chapterService;
    private readonly UserService _userService;
    private readonly GoogleCloudService _googleCloudService;
    private readonly IMapper _mapper;
    private readonly AddToCache _addToCache;
    
    private readonly EmailService _emailService;
    private readonly EmailMessages _emailMessages;
    private readonly BotMessage _botMessage;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="emailService">Сервис Email.</param>
    /// <param name="emailMessages">EmailMessages.</param>
    /// <param name="workRepository">Репозиторий Work.</param>
    /// <param name="googleCloudService">Сервис GoogleCloud.</param>
    /// <param name="mapper">Автомаппер.</param>
    /// <param name="botMessage">BotMessages.</param>
    /// <param name="addToCache">AddToCache.</param>
    /// <param name="workTagService">Сервис WorkTag.</param>
    /// <param name="workLikeService">Сервис WorkLike.</param>
    /// <param name="chapterService">Сервис Chapter.</param>
    /// <param name="userService">Сервис User.</param>
    public WorkService(
        EmailService emailService,
        EmailMessages emailMessages,
        IWorkRepository workRepository,
        GoogleCloudService googleCloudService,
        IMapper mapper,
        BotMessage botMessage,
        AddToCache addToCache,
        WorkTagService workTagService,
        WorkLikeService workLikeService,
        ChapterService chapterService,
        UserService userService)
    {
        _workRepository = Guard.Against.Null(workRepository);

        _workTagService = Guard.Against.Null(workTagService);
        _workLikeService = Guard.Against.Null(workLikeService);
        _chapterService = Guard.Against.Null(chapterService);
        _googleCloudService = Guard.Against.Null(googleCloudService);
        _mapper = Guard.Against.Null(mapper);
        _addToCache = Guard.Against.Null(addToCache);
        
        _botMessage = Guard.Against.Null(botMessage);
        _emailService = Guard.Against.Null(emailService);
        _emailMessages = Guard.Against.Null(emailMessages);
        _userService = Guard.Against.Null(userService);
    }

    /// <summary>
    /// Создание Work
    /// </summary>
    /// <param name="workRequest">Данные для Work.</param>
    /// <param name="fileStream">Обложка.</param>
    /// <param name="fileName">Название файла.</param>
    /// <param name="contentType">Тип файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новая Work.</returns>
    public async Task<CreateWorkResponse> CreateAsync(
        CreateWorkRequest workRequest,
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(workRequest);
        var work = _mapper.Map<Work>(workRequest);
        
        work.CoverUrl = await _googleCloudService.UploadFileAsync(fileStream, fileName + $"-{work.Id.ToString()}", contentType);
        await _addToCache.StoreInCache(work, "CACHED-WORKS-CREATE", cancellationToken);

        var workTagRequests = workRequest.TagIs.Select(tagId => new WorkTagRequest { WorkId = work.Id, TagId = tagId }).ToList();
        await _workTagService.CreateAsync(workTagRequests, cancellationToken);
        
        await _chapterService.CreateAsync(workRequest.ChapterRequest, work.Id, cancellationToken);
        
        var user = await _userService.GetByIdAsync(work.UserId, cancellationToken);
        await _emailService.CreateEmailAsync(
            user.Email,
            _emailMessages.GetWorkCreateTheme(work),
            _emailMessages.GetWorkCreateMessage(work),
            cancellationToken);
        
        return _mapper.Map<CreateWorkResponse>(work);
    }
    
    /// <summary>
    /// Обновление Work
    /// </summary>
    /// <param name="workRequest">Данные для обновления Work.</param>
    /// <param name="fileStream">Обложка.</param>
    /// <param name="fileName">Название файла.</param>
    /// <param name="contentType">Тип файла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленная Work.</returns>
    public async Task<UpdateWorkResponse> UpdateAsync(
        UpdateWorkRequest workRequest,
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(workRequest);
        var work = await GetByIdWork(workRequest.Id, cancellationToken);

        await _googleCloudService.DeleteFileAsync(work.CoverUrl.Split("/").Last());
        workRequest.CoverUrl = await _googleCloudService.UploadFileAsync(fileStream, fileName + $"-{work.Id.ToString()}", contentType);
        await _addToCache.StoreInCache(workRequest, "CACHED-WORKS-UPDATE", cancellationToken);
        
        var workTagDeleteRequests = work.WorkTags.Select(wt => new WorkTagRequest { WorkId = work.Id, TagId = wt.TagId }).ToList();
        await _workTagService.DeleteAsync(workTagDeleteRequests, cancellationToken);
        
        var workTagCreateRequests = workRequest.TagIs.Select(tagId => new WorkTagRequest { WorkId = work.Id, TagId = tagId }).ToList();
        await _workTagService.UpdateAsync(workTagCreateRequests, cancellationToken);

        return _mapper.Map<UpdateWorkResponse>(work);
    }
    
    /// <summary>
    /// Удаление Work
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DeleteAsync(Guid workId, CancellationToken cancellationToken)
    {
        var work = await GetByIdWork(workId, cancellationToken);
        await _workRepository.DeleteAsync(work, cancellationToken);
        
        await _googleCloudService.DeleteFileAsync(work.CoverUrl.Split("/").Last());
        await _workRepository.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Увеличение лайка
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task IncrementLikeAsync(Guid userId, Guid workId, CancellationToken cancellationToken)
    {
        if (await _workLikeService.HasUserLikedWorkAsync(userId, workId, cancellationToken))
            throw new InvalidOperationException("Пользователь уже поставил лайк на эту работу.");

        var work = await _workRepository.GetByIdAsync(workId, cancellationToken);
        work.IncrementLike();
        await _workRepository.SaveChangesAsync(cancellationToken);
        
        await _workLikeService.CreateAsync(new WorkLikeRequest { UserId = userId, WorkId = workId }, cancellationToken);
    }
    
    /// <summary>
    /// Уменьшение лайка
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task DecrementLikeAsync(Guid userId, Guid workId, CancellationToken cancellationToken)
    {
        if (!await _workLikeService.HasUserLikedWorkAsync(userId, workId, cancellationToken))
            throw new InvalidOperationException("Пользователь не поставил лайк на эту работу.");
        
        var work = await _workRepository.GetByIdAsync(workId, cancellationToken);
        work.DecrementLike();
        await _workRepository.SaveChangesAsync(cancellationToken);
        
        await _workLikeService.DeleteAsync(new WorkLikeRequest { UserId = userId, WorkId = workId }, cancellationToken);
    }
    
    /// <summary>
    /// Получение Work по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Work.</returns>
    public async Task<GetWorkResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var work = await GetByIdWork(userId, cancellationToken);
        return _mapper.Map<GetWorkResponse>(work);
    }
    
    /// <summary>
    /// Получение всех Work
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    public async Task<List<GetWorkResponse>> GetAllWorkAsync(CancellationToken cancellationToken)
    {
        var works = await _workRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<GetWorkResponse>>(works);
    }
    
    /// <summary>
    /// Получение всех Work пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    public async Task<List<GetWorkResponse>> GetAllUserWorkAsync(Guid userId, CancellationToken cancellationToken)
    {
        var works = await _workRepository.GetAllUserWorkAsync(userId, cancellationToken);
        return _mapper.Map<List<GetWorkResponse>>(works);
    }
    
    private async Task<Work> GetByIdWork(Guid id, CancellationToken cancellationToken)
    { 
        var work = await _workRepository.GetByIdAsync(id, cancellationToken);
        if (work == null)
            return null;

        return work;
    }
}