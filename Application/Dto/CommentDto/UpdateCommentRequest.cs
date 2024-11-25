namespace Application.Dto.CommentDto;

public class UpdateCommentRequest
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; init; }
}