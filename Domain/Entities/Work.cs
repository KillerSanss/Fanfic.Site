using Domain.Primitives;
using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Сущность работы
/// </summary>
public class Work : BaseEntity
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство User
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Дата публикации
    /// </summary>
    public DateTime PublicationDate { get; set; }
    
    /// <summary>
    /// Категория
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Кол-во лайков
    /// </summary>
    public int Likes { get; set; }

    /// <summary>
    /// Кол-во просмотров
    /// </summary>
    public int Views { get; set; }
    
    /// <summary>
    /// Обложка
    /// </summary>
    public string CoverUrl { get; set; }
    
    /// <summary>
    /// Комментарии
    /// </summary>
    public ICollection<Chapter> Chapters = new List<Chapter>();
    
    /// <summary>
    /// Список лайков
    /// </summary>
    public ICollection<WorkLike> WorkLikes = new List<WorkLike>();
    
    /// <summary>
    /// Тэги
    /// </summary>
    public ICollection<WorkTag> WorkTags = new List<WorkTag>();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="title">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="category">Категория.</param>
    /// <param name="coverUrl">Обложка.</param>
    public Work(
        Guid id,
        Guid userId,
        string title,
        string description,
        Category category,
        string coverUrl)
    {
        SetId(id);
        UserId = userId;
        Title = title;
        Description = description;
        PublicationDate = DateTime.Now;
        Category = category;
        CoverUrl = coverUrl;
        
        Validate();
    }

    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="title">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="category">Категория.</param>
    /// <param name="coverUrl">Обложка.</param>
    /// <returns>Обновленная работа.</returns>
    public Work Update(
        string title,
        string description,
        Category category,
        string coverUrl)
    {
        Title = title;
        Description = description;
        Category = category;
        CoverUrl = coverUrl;
        
        WorkTags.Clear();

        Validate();
        
        return this;
    }
    
    /// <summary>
    /// Увеличивает количество лайков на 1.
    /// </summary>
    public void IncrementLike()
    {
        Likes++;
        
        Validate();
    }

    /// <summary>
    /// Уменьшает количество лайков на 1.
    /// </summary>
    public void DecrementLike()
    {
        Likes--;
        
        Validate();
    }

    /// <summary>
    /// Увеличивает количество просмотров на 1.
    /// </summary>
    public void IncrementView()
    {
        Views++;
        
        Validate();
    }
    
    private void Validate()
    {
        var validator = new WorkValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }

    public Work()
    {
    }
}