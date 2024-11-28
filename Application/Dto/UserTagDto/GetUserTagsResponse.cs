namespace Application.Dto.UserTagDto;

/// <summary>
/// Дто ответа на получение любимых тэгов User
/// </summary>
public class GetUserTagsResponse
{
    /// <summary>
    /// Идентификатор User
    /// </summary>
    public Guid UserId { get; init; }
}