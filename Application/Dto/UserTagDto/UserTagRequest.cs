namespace Application.Dto.UserTagDto;

/// <summary>
/// Дто запроса на создание UserTag
/// </summary>
public class UserTagRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор тэга
    /// </summary>
    public Guid TagId { get; init; }
}