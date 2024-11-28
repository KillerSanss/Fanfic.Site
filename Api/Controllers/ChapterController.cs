using System.Security.Claims;
using Application.Dto.ChapterDto;
using Application.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер Chapter
/// </summary>
[Route("api/chapters")]
[ApiController]
public class ChapterController : ControllerBase
{
    private readonly ChapterService _chapterService;
    private readonly WorkService _workService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="chapterService">ChapterService.</param>
    /// <param name="workService">WorkService.</param>
    public ChapterController(
        ChapterService chapterService,
        WorkService workService)
    {
        _chapterService = Guard.Against.Null(chapterService);
        _workService = Guard.Against.Null(workService);
    }

    /// <summary>
    /// Создание Chapter
    /// </summary>
    /// <param name="chapterRequest">Данные для Chapter.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый Chapter.</returns>
    [Authorize]
    [HttpPost("create/{workId:guid}")]
    public async Task<ActionResult> CreateChapter(
        [FromForm] CreateChapterRequest chapterRequest,
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var work = await _workService.GetByIdAsync(workId, cancellationToken);
        if (work.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете добавить главу к работе другого пользователя.");
        
        var addedChapter = await _chapterService.CreateAsync(chapterRequest, workId, cancellationToken);
        return Created(nameof(CreateChapter), addedChapter);
    }

    /// <summary>
    /// Обновление Chapter
    /// </summary>
    /// <param name="updateChapterRequest">Данные для обновления Chapter.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный Chapter.</returns>
    [Authorize]
    [HttpPut("update/{workId:guid}")]
    public async Task<ActionResult<UpdateChapterResponse>> UpdateChapter(
        [FromForm] UpdateChapterRequest updateChapterRequest,
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var work = await _workService.GetByIdAsync(workId, cancellationToken);
        if (work.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете обновить главу в работе другого пользователя.");
        
        var updatedChapter = await _chapterService.UpdateAsync(updateChapterRequest, cancellationToken);
        return Ok(updatedChapter);
    }

    /// <summary>
    /// Удаление Chapter
    /// </summary>
    /// <param name="chapterId">Идентификатор Chapter.</param>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpDelete("delete/{chapterId:guid}/{workId:guid}")]
    public async Task<ActionResult> DeleteChapter(
        [FromRoute] Guid chapterId,
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var work = await _workService.GetByIdAsync(workId, cancellationToken);
        if (work.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете удалить главу в работе другого пользователя.");
        
        await _chapterService.DeleteAsync(chapterId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Получение Chapter по идентификатору
    /// </summary>
    /// <param name="chapterId">Идентификатор Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Chapter.</returns>
    [HttpGet("{chapterId:guid}")]
    public async Task<ActionResult<GetChapterResponse>> GetByIdChapter(
        [FromRoute] Guid chapterId,
        CancellationToken cancellationToken)
    {
        var chapter = await _chapterService.GetByIdAsync(chapterId, cancellationToken);
        return Ok(chapter);
    }
    
    /// <summary>
    /// Получение всех Chapter
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    [HttpGet]
    public async Task<ActionResult<List<GetChapterResponse>>> GetAllChapter(
        CancellationToken cancellationToken)
    {
        var chapters = await _chapterService.GetAllChapterAsync(cancellationToken);
        return Ok(chapters);
    }

    /// <summary>
    /// Получение всех Chapter работы
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Chapter.</returns>
    [HttpGet("all/{workId:guid}")]
    public async Task<ActionResult<List<GetChapterResponse>>> GetAllWorkChapter(
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var chapters = await _chapterService.GetAllWorkChapterAsync(workId, cancellationToken);
        return Ok(chapters);
    }
}