using Application.Dto.CommentDto;
using Domain.Entities;

namespace Application.Dto.ChapterDto;

/// <summary>
/// Дто ответа на получение Chapter
/// </summary>
public class GetChapterResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор работы
    /// </summary>
    public Guid WorkId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// Комментарии
    /// </summary>
    public ICollection<GetChapterCommentResponse> Comments { get; init; }
}    