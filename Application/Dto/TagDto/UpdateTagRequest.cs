using Domain.Primitives;

namespace Application.Dto.TagDto;

/// <summary>
/// Дто запроса на обновление Tag
/// </summary>
public class UpdateTagRequest
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    /// Возрастное ограничение
    /// </summary>
    public AgeRestriction AgeRestriction { get; init; }
    
    /// <summary>
    /// Идентификатор User
    /// </summary>
    public Guid UserId { get; init; }
}