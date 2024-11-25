using Application.Dto.ChapterDto;
using Domain.Primitives;

namespace Application.Dto.WorkDto;

/// <summary>
/// Дто запроса на создание Work
/// </summary>
public class CreateWorkRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }
    
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
    /// Список тэгов
    /// </summary>
    public ICollection<Guid> TagIs { get; init; }
    
    /// <summary>
    /// Данные главы
    /// </summary>
    public CreateChapterRequest ChapterRequest { get; set; }
}