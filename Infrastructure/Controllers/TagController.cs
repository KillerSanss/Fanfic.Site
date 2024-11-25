using System.Security.Claims;
using Application.Dto.TagDto;
using Application.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

/// <summary>
/// Контроллер Tag
/// </summary>
[Route("api/tags")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="tagService">TagService.</param>
    public TagController(
        TagService tagService)
    {
        _tagService = Guard.Against.Null(tagService);
    }
    
    /// <summary>
    /// Создание Tag
    /// </summary>
    /// <param name="tagRequest">Данные для Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый Tag.</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<ActionResult> CreateTag(
        [FromForm] CreateTagRequest tagRequest,
        CancellationToken cancellationToken)
    {
        var addedTag = await _tagService.CreateAsync(tagRequest, cancellationToken);
        return Created(nameof(CreateTag), addedTag);
    }
    
    /// <summary>
    /// Обновление Tag
    /// </summary>
    /// <param name="updateTagRequest">Данные для обновления Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный Tag.</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<ActionResult<UpdateTagResponse>> UpdateTag(
        [FromForm] UpdateTagRequest updateTagRequest,
        CancellationToken cancellationToken)
    {
        var updatedTag = await _tagService.UpdateAsync(updateTagRequest, cancellationToken);
        return Ok(updatedTag);
    }

    /// <summary>
    /// Удаление Tag
    /// </summary>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{tagId:guid}")]
    public async Task<ActionResult> DeleteTag(
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        await _tagService.DeleteAsync(tagId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Добавление Tag в избранное
    /// </summary>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpPost("follow/{tagId:guid}")]
    public async Task<ActionResult> FollowTag(
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _tagService.FollowTagAsync(Guid.Parse(userIdClaim), tagId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Удаление Tag из избранного
    /// </summary>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpPost("unfollow/{tagId:guid}")]
    public async Task<ActionResult> UnfollowTag(
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _tagService.UnfollowTagAsync(Guid.Parse(userIdClaim), tagId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Получение Tag по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор Tag.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Tag.</returns>
    [HttpGet("{tagId:guid}")]
    public async Task<ActionResult<GetTagResponse>> GetByIdTag(
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var tag = await _tagService.GetByIdAsync(tagId, cancellationToken);
        return Ok(tag);
    }
    
    /// <summary>
    /// Получение всех Tag
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Tag.</returns>
    [HttpGet]
    public async Task<ActionResult<List<GetTagResponse>>> GetAllTag(
        CancellationToken cancellationToken)
    {
        var tags = await _tagService.GetAllTagAsync(cancellationToken);
        return Ok(tags);
    }
}