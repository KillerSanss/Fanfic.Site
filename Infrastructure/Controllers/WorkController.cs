using System.Security.Claims;
using Application.Dto.WorkDto;
using Application.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

/// <summary>
/// Контроллер Work
/// </summary>
[Route("api/works")]
[ApiController]
public class WorkController : ControllerBase
{
    private readonly WorkService _workService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="workService">WorkService.</param>
    public WorkController(
        WorkService workService)
    {
        _workService = Guard.Against.Null(workService);
    }

    /// <summary>
    /// Создание Work
    /// </summary>
    /// <param name="workRequest">Данные для Work.</param>
    /// <param name="coverFile">Файл обложки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новая Work.</returns>
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateWork(
        [FromForm] CreateWorkRequest workRequest,
        IFormFile coverFile, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var workUser = await _workService.GetByIdAsync(workRequest.UserId, cancellationToken);
        if (workUser.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете добавить работу для другого пользователя");
        
        var stream = coverFile.OpenReadStream();
        {
            var createResponse = await _workService.CreateAsync(
                workRequest,
                stream,
                coverFile.FileName,
                coverFile.ContentType,
                cancellationToken);

            return Created(nameof(CreateWork), createResponse);
        }
    }
    
    /// <summary>
    /// Обновление Work
    /// </summary>
    /// <param name="updateWorkRequest">Данные для обновления Work.</param>
    /// <param name="coverFile">Файл обложки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленная Work.</returns>
    [Authorize]
    [HttpPut("update")]
    public async Task<ActionResult<UpdateWorkResponse>> UpdateWork(
        [FromForm] UpdateWorkRequest updateWorkRequest,
        IFormFile coverFile,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var work = await _workService.GetByIdAsync(updateWorkRequest.Id, cancellationToken);
        if (work.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете обновить работу другого пользователя.");
        
        var stream = coverFile.OpenReadStream();
        {
            var updatedWork = await _workService.UpdateAsync(
                updateWorkRequest,
                stream,
                coverFile.FileName,
                coverFile.ContentType,
                cancellationToken);
            return Ok(updatedWork);
        }
    }
    
    /// <summary>
    /// Удаление Work
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpDelete("delete/{workId:guid}")]
    public async Task<ActionResult> DeleteWork(
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var work = await _workService.GetByIdAsync(workId, cancellationToken);
        if (work.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете удалить работу другого пользователя.");
        
        await _workService.DeleteAsync(workId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Добавление лайка к Work
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpPost("like/{workId:guid}")]
    public async Task<ActionResult> IncrementLike(
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        await _workService.IncrementLikeAsync(Guid.Parse(userIdClaim), workId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удаление лайка у Work
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpPost("dislike/{workId:guid}")]
    public async Task<ActionResult> DecrementLike(
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        await _workService.DecrementLikeAsync(Guid.Parse(userIdClaim), workId, cancellationToken);
        return NoContent();
    }

    
    /// <summary>
    /// Получение Work по идентификатору
    /// </summary>
    /// <param name="workId">Идентификатор Work.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Work.</returns>
    [HttpGet("{workId:guid}")]
    public async Task<ActionResult<GetWorkResponse>> GetByIdWork(
        [FromRoute] Guid workId,
        CancellationToken cancellationToken)
    {
        var work = await _workService.GetByIdAsync(workId, cancellationToken);
        return Ok(work);
    }
    
    /// <summary>
    /// Получение всех Work
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<GetWorkResponse>>> GetAllWork(
        CancellationToken cancellationToken)
    {
        var works = await _workService.GetAllWorkAsync(cancellationToken);
        return Ok(works);
    }

    /// <summary>
    /// Получение всех Work пользователя
    /// </summary>
    /// <param name="userId">Идентификатор User.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Work.</returns>
    [HttpGet("all/{userId:guid}")]
    public async Task<ActionResult<List<GetWorkResponse>>> GetAllUserWork(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var works = await _workService.GetAllUserWorkAsync(userId, cancellationToken);
        return Ok(works);
    }
}