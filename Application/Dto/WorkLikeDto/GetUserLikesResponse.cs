namespace Application.Dto.WorkLikeDto;

/// <summary>
/// Дто ответа на получение лайков User
/// </summary>
public class GetUserLikesResponse
{
    /// <summary>
    /// Идентификатор Work
    /// </summary>
    public Guid WorkId { get; init; }
}