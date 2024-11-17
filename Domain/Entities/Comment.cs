using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Сущность комментария
/// </summary>
public class Comment : BaseEntity
{
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство User
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// Идентификатор главы
    /// </summary>
    public Guid ChapterId { get; set; }
    
    /// <summary>
    /// Навигационное свойство Chapter
    /// </summary>
    public Chapter Chapter { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="content">Содержание.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="chapterId">Идентификатор главы.</param>
    public Comment(
        Guid id,
        string content,
        Guid userId,
        Guid chapterId)
    {
        SetId(id);
        Content = content;
        UserId = userId;
        ChapterId = chapterId;
        
        Validate();
    }
    
    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="content">Содержание.</param>
    /// <returns>Обновленный комментарий.</returns>
    public Comment Update(
        string content)
    {
        Content = content;
        
        Validate();
        
        return this;
    }
    
    private void Validate()
    {
        var validator = new CommentValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }

    public Comment()
    {
    }
}