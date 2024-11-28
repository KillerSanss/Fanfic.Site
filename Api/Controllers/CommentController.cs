using System.Security.Claims;
using Application.Dto.CommentDto;
using Application.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер Comment
/// </summary>
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="commentService">CommentService.</param>
    public CommentController(
        CommentService commentService)
    {
        _commentService = Guard.Against.Null(commentService);
    }
    
    /// <summary>
    /// Создание Comment
    /// </summary>
    /// <param name="commentRequest">Данные для Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Новый Comment.</returns>
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateComment(
        [FromForm] CreateCommentRequest commentRequest,
        CancellationToken cancellationToken)
    {
        var addedComment = await _commentService.CreateAsync(commentRequest, cancellationToken);
        return Created(nameof(CreateComment), addedComment);
    }
    
    /// <summary>
    /// Обновление Comment
    /// </summary>
    /// <param name="updateCommentRequest">Данные для обновления Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный Comment.</returns>
    [Authorize]
    [HttpPut("update")]
    public async Task<ActionResult<UpdateCommentResponse>> UpdateComment(
        [FromForm] UpdateCommentRequest updateCommentRequest,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var comment = await _commentService.GetByIdAsync(updateCommentRequest.Id, cancellationToken);
        if (comment.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете обновить комментарий другого пользователя.");
        
        var updatedComment = await _commentService.UpdateAsync(updateCommentRequest, cancellationToken);
        return Ok(updatedComment);
    }
    
    /// <summary>
    /// Удаление Comment
    /// </summary>
    /// <param name="commentId">Идентификатор Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    [Authorize]
    [HttpDelete("delete/{commentId:guid}")]
    public async Task<ActionResult> DeleteTag(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var comment = await _commentService.GetByIdAsync(commentId, cancellationToken);
        if (comment.UserId != Guid.Parse(userIdClaim))
            return Forbid("Вы не можете удалить комментарий другого пользователя.");
        
        await _commentService.DeleteAsync(commentId, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Получение Comment по идентификатору
    /// </summary>
    /// <param name="commentId">Идентификатор Comment.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Comment.</returns>
    [HttpGet("{commentId:guid}")]
    public async Task<ActionResult<GetCommentResponse>> GetByIdChapter(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var comment = await _commentService.GetByIdAsync(commentId, cancellationToken);
        return Ok(comment);
    }
    
    /// <summary>
    /// Получение всех Comment
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    [HttpGet]
    public async Task<ActionResult<List<GetCommentResponse>>> GetAllChapter(
        CancellationToken cancellationToken)
    {
        var comments = await _commentService.GetAllCommentAsync(cancellationToken);
        return Ok(comments);
    }

    /// <summary>
    /// Получение всех Comment у главы
    /// </summary>
    /// <param name="chapterId">Идентификатор Chapter.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список Comment.</returns>
    [HttpGet("all/{chapterId:guid}")]
    public async Task<ActionResult<List<GetCommentResponse>>> GetAllChapterComments(
        [FromRoute] Guid chapterId,
        CancellationToken cancellationToken)
    {
        var comments = await _commentService.GetAllChapterCommentAsync(chapterId, cancellationToken);
        return Ok(comments);
    }
}