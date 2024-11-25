namespace Application.Dto.CommentDto;

public class GetCommentResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
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