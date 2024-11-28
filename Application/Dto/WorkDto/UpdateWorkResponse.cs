using Domain.Primitives;

namespace Application.Dto.WorkDto;

/// <summary>
/// Дто ответа на обновление Work
/// </summary>
public class UpdateWorkResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
    
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
    /// Дата публикации
    /// </summary>
    public DateTime PublicationDate { get; init; }
    
    /// <summary>
    /// Категория
    /// </summary>
    public Category Category { get; init; }

    /// <summary>
    /// Кол-во лайков
    /// </summary>
    public int Likes { get; init; }

    /// <summary>
    /// Кол-во просмотров
    /// </summary>
    public int Views { get; init; }
    
    /// <summary>
    /// Обложка
    /// </summary>
    public string CoverUrl { get; init; }
}