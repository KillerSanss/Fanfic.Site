using Domain.Entities;
using FluentValidation;

namespace Domain.Validations.Validators;

/// <summary>
/// Валидация Work
/// </summary>
public class WorkValidator : AbstractValidator<Work>
{
    public WorkValidator()
    {
        RuleFor(d => d.UserId)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
        
        RuleFor(d => d.Title)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(1).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(30).WithMessage(ValidationMessages.MaximumLengthError);
        
        RuleFor(d => d.Description)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(1).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(500).WithMessage(ValidationMessages.MaximumLengthError);
        
        RuleFor(d => d.PublicationDate)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .LessThanOrEqualTo(DateTime.Now).WithMessage(ValidationMessages.DateError);
        
        RuleFor(d => d.Category)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .IsInEnum().WithMessage(ValidationMessages.EnumError);
        
        RuleFor(d => d.Likes)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.NegativeNumberError);
        
        RuleFor(d => d.Views)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.NegativeNumberError);

        RuleFor(d => d.CoverUrl)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
    }
}