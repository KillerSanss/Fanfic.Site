using Domain.Entities;
using FluentValidation;

namespace Domain.Validations.Validators;

/// <summary>
/// Валидация User
/// </summary>
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(d => d.NickName)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(2).WithMessage(ValidationMessages.MinimumLengthError)
            .MaximumLength(15).WithMessage(ValidationMessages.MaximumLengthError);

        RuleFor(d => d.RegistrationDate)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .LessThanOrEqualTo(DateTime.Now).WithMessage(ValidationMessages.DateError);
        
        RuleFor(d => d.BirthDate)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .LessThanOrEqualTo(DateTime.Now).WithMessage(ValidationMessages.DateError);

        RuleFor(d => d.Gender)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .IsInEnum().WithMessage(ValidationMessages.EnumError);

        RuleFor(d => d.Email)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .Matches(RegexPatterns.EmailPattern).WithMessage(ValidationMessages.EmailError);

        RuleFor(d => d.Password)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .MinimumLength(8).WithMessage(ValidationMessages.MinimumLengthError);
        
        RuleFor(d => d.Role)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError)
            .IsInEnum().WithMessage(ValidationMessages.EnumError);

        RuleFor(d => d.AvatarUrl)
            .NotNull().WithMessage(ValidationMessages.NullError)
            .NotEmpty().WithMessage(ValidationMessages.EmptyError);
    }
}