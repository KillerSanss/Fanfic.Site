namespace Application.Dto.ChapterDto;

/// <summary>
/// Дто запроса на создание Chapter
/// </summary>
public class CreateChapterRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
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