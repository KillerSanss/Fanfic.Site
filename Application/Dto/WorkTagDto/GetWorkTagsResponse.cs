namespace Application.Dto.WorkTagDto;

/// <summary>
/// Дто ответа на получение тэгов Work
/// </summary>
public class GetWorkTagsResponse
{    
    /// <summary>
    /// Идентификатор Tag
    /// </summary>
    public Guid TagId { get; init; }
}