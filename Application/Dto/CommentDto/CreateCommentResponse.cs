namespace Application.Dto.CommentDto;

public class CreateCommentResponse
{
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; init; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор главы
    /// </summary>
    public Guid ChapterId { get; init; }
    
    /// <summary>
    /// Идентификатор родительского комментарий
    /// </summary>
    public Guid? ParentCommentId { get; init; }
}