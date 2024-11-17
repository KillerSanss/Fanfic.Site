using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Промежуточная сущность для связи User и Tag
/// </summary>
public class UserTag : BaseEntity
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
    /// Идентификатор тэга
    /// </summary>
    public Guid TagId { get; set; }
    
    /// <summary>
    /// Навигационное свойство Tag
    /// </summary>
    public Tag Tag { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="tagId">Идентификатор тэга.</param>
    public UserTag(
        Guid id,
        Guid userId,
        Guid tagId)
    {
        SetId(id);
        UserId = userId;
        TagId = tagId;
        
        Validate();
    }
    
    private void Validate()
    {
        var validator = new UserTagValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }
    
    public UserTag()
    {
    }
}