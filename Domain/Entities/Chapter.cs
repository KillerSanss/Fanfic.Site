using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Сущность главы
/// </summary>
public class Chapter : BaseEntity
{
    /// <summary>
    /// Идентификатор работы
    /// </summary>
    public Guid WorkId { get; set; }
    
    /// <summary>
    /// Навигационное свойство Work
    /// </summary>
    public Work Work { get; set; }
    
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
    /// Содержание
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// Комментарии
    /// </summary>
    public readonly List<Comment> Comments = new();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="title">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="content">Содержание.</param>
    public Chapter(
        Guid id,
        Guid workId,
        Guid userId,
        string title,
        string description,
        string content)
    {
        SetId(id);
        WorkId = workId;
        UserId = userId;
        Title = title;
        Description = description;
        Content = content;

        Validate();
    }

    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="title">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="content">Содержание.</param>
    /// <returns>Обновленная глава.</returns>
    public Chapter Update(
        string title,
        string description,
        string content)
    {
        Title = title;
        Description = description;
        Content = content;
        
        Validate();
        
        return this;
    }
    
    private void Validate()
    {
        var validator = new ChapterValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }

    public Chapter()
    {
    }
}