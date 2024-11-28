namespace Application.Dto.WorkTagDto;

/// <summary>
/// Дто ответа на получение всех работ с Tag
/// </summary>
public class GetTagWorksResponse
{
    /// <summary>
    /// Идентификатор Work
    /// </summary>
    public Guid WorkId { get; init; }
}