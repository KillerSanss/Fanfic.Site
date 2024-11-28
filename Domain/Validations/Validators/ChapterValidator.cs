using Domain.Entities;
using FluentValidation;

namespace Domain.Validations.Validators;

/// <summary>
/// Валидация Chapter
/// </summary>
public class ChapterValidator : AbstractValidator<Chapter>
{
    public ChapterValidator()
    {
        RuleFor(d => d.WorkId)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
        
        RuleFor(d => d.UserId)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
        
        RuleFor(d => d.Title)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(2).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(30).WithMessage(ValidationMessages.MaximumLengthError);
        
        RuleFor(d => d.Description)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(1).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(500).WithMessage(ValidationMessages.MaximumLengthError);
        
        RuleFor(d => d.Content)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(1).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(25000).WithMessage(ValidationMessages.MaximumLengthError);
    }
}