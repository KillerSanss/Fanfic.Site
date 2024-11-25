namespace Application.Dto.WorkTagDto;

/// <summary>
/// Дто запроса на создание WorkTag
/// </summary>
public class WorkTagRequest
{
    /// <summary>
    /// Идентификатор работы
    /// </summary>
    public Guid WorkId { get; init; }
    
    /// <summary>
    /// Идентификатор тэга
    /// </summary>
    public Guid TagId { get; init; }
}