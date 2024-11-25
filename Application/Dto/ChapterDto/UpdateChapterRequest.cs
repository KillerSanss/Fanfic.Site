namespace Application.Dto.ChapterDto;

/// <summary>
/// Дто запроса на обновление Chapter
/// </summary>
public class UpdateChapterRequest
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; set; }
}