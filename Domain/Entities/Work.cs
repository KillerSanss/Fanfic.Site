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
    public DateTime PublicationDate { get; set; } = DateTime.Now;
    
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
    public ICollection<Chapter> Chapters { get; set; }
    
    /// <summary>
    /// Список лайков
    /// </summary>
    public ICollection<WorkLike> WorkLikes { get; set; }
    
    /// <summary>
    /// Тэги
    /// </summary>
    public ICollection<WorkTag> WorkTags { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="title">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="category">Категория.</param>
    public Work(
        Guid id,
        Guid userId,
        string title,
        string description,
        Category category)
    {
        SetId(id);
        UserId = userId;
        Title = title;
        Description = description;
        Category = category;
        
        Validate();
    }

    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="title">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="category">Категория.</param>
    /// <returns>Обновленная работа.</returns>
    public Work Update(
        string title,
        string description,
        Category category)
    {
        Title = title;
        Description = description;
        Category = category;

        Validate();
        
        return this;
    }
    

    /// <summary>
    /// Увеличивает количество лайков
    /// </summary>
    public Work IncrementLike()
    {
        Likes++;

        return this;
    }

    /// <summary>
    /// Уменьшает количество лайков
    /// </summary>
    public Work DecrementLike()
    {
        Likes--;

        return this;
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