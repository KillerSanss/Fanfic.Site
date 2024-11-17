using Domain.Entities;
using FluentValidation;

namespace Domain.Validations.Validators;

/// <summary>
/// Валидация WorkTag
/// </summary>
public class WorkTagValidator : AbstractValidator<WorkTag>
{
    public WorkTagValidator()
    {
        RuleFor(d => d.WorkId)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
        
        RuleFor(d => d.TagId)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
    }
}