namespace Application.Dto.WorkLikeDto;

/// <summary>
/// Дто ответа на получение лайков Work
/// </summary>
public class GetWorkLikesResponse
{
    /// <summary>
    /// Идентификатор User
    /// </summary>
    public Guid UserId { get; init; }
}