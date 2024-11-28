using Domain.Primitives;

namespace Application.Dto.TagDto;

/// <summary>
/// Дто ответа на создание Tag
/// </summary>
public class CreateTagResponse
{
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
}