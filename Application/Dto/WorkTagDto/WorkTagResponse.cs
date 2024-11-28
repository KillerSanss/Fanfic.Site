namespace Application.Dto.WorkTagDto;

/// <summary>
/// Дто ответа на создание WorkTag
/// </summary>
public class WorkTagResponse
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