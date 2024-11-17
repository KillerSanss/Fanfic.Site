using Domain.Primitives;
using Domain.Validations.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Сущность тэга
/// </summary>
public class Tag : BaseEntity
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Возрастное ограничение
    /// </summary>
    public AgeRestriction AgeRestriction { get; set; }
    
    /// <summary>
    /// Привязка к работам
    /// </summary>
    public ICollection<WorkTag> WorkTags { get; set; } = new List<WorkTag>();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="name">Название.</param>
    /// <param name="ageRestriction">Возрастное ограничение.</param>
    /// <param name="description">Описание.</param>
    public Tag(
        Guid id,
        string name,
        AgeRestriction ageRestriction,
        string description)
    {
        SetId(id);
        Name = name;
        AgeRestriction = ageRestriction;
        Description = description;
        
        Validate();
    }

    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="name">Название.</param>
    /// <param name="ageRestriction">Возрастное ограничение.</param>
    /// <param name="description">Описание.</param>
    /// <returns>Обновленный тэг.</returns>
    public Tag Update(
        string name,
        AgeRestriction ageRestriction,
        string description)
    {
        Name = name;
        AgeRestriction = ageRestriction;
        Description = description;
        
        Validate();
        
        return this;
    }
    
    private void Validate()
    {
        var validator = new TagValidator();
        var result = validator.Validate(this);

        if (!result.IsValid)
        {
            var errors = string.Join(" || ", result.Errors.Select(x => x.ErrorMessage));
            throw new ValidationException(errors);
        }
    }

    public Tag()
    {
    }
}