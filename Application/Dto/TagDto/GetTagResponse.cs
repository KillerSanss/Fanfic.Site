using Application.Dto.WorkTagDto;
using Domain.Primitives;

namespace Application.Dto.TagDto;

public class GetTagResponse
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
    /// Привязка к работам
    /// </summary>
    public ICollection<GetTagWorksResponse> TagWorks { get; init; }
}