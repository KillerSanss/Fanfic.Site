using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Промежуточная сущность для связи Work и Tag
/// </summary>
public class WorkTag
{
    /// <summary>
    /// Идентификатор работы
    /// </summary>
    public Guid WorkId { get; set; }
    
    /// <summary>
    /// Работа
    /// </summary>
    public Work Work { get; set; }
    
    /// <summary>
    /// Идентификатор тэга
    /// </summary>
    public Guid TagId { get; set; }
    
    /// <summary>
    /// Тэг
    /// </summary>
    public Tag Tag { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="workId">Идентификатор работы.</param>
    /// <param name="tagId">Идентификатор тэга.</param>
    public WorkTag(
        Guid workId,
        Guid tagId)
    {
        WorkId = workId;
        TagId = tagId;
        
        Validate();
    }
    
    private void Validate()
    {
        var validator = new WorkTagValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }
    
    public WorkTag()
    {
    }
}