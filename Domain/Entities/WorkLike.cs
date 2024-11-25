using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Промежуточная сущность для связи лайка и Work
/// </summary>
public class WorkLike
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
    /// Идентификатор работы
    /// </summary>
    public Guid WorkId { get; set; }
    
    /// <summary>
    /// Навигационное свойство Work
    /// </summary>
    public Work Work { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="workId">Идентификатор работы.</param>
    public WorkLike(
        Guid userId,
        Guid workId)
    {
        UserId = userId;
        WorkId = workId;
        
        Validate();
    }
    
    private void Validate()
    {
        var validator = new WorkLikeValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }
    
    public WorkLike()
    {
    }
}