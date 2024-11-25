using Domain.Primitives;

namespace Application.Dto.WorkDto;

/// <summary>
/// Дто запроса на обновление Work
/// </summary>
public class UpdateWorkRequest
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; init; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    /// Категория
    /// </summary>
    public Category Category { get; init; }
    
    /// <summary>
    /// Обложка
    /// </summary>
    public string CoverUrl { get; set; }
    
    /// <summary>
    /// Список тэгов
    /// </summary>
    public List<Guid> TagIs { get; init; }
}