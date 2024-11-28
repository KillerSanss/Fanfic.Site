namespace Application.Dto.WorkLikeDto;

/// <summary>
/// Дто запроса на создание WorkLike
/// </summary>
public class WorkLikeRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор работы
    /// </summary>
    public Guid WorkId { get; init; }
}