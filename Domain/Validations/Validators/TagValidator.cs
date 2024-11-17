using Domain.Entities;
using FluentValidation;

namespace Domain.Validations.Validators;

/// <summary>
/// Валидация Tag
/// </summary>
public class TagValidator : AbstractValidator<Tag>
{
    public TagValidator()
    {
        RuleFor(d => d.Name)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(2).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(30).WithMessage(ValidationMessages.MaximumLengthError);

        RuleFor(d => d.Description)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(2).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(500).WithMessage(ValidationMessages.MaximumLengthError);
        
        RuleFor(d => d.AgeRestriction)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .IsInEnum().WithMessage(ValidationMessages.EnumError);
    }
}