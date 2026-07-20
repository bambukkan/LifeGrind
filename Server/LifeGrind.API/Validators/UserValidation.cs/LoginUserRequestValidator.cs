using FluentValidation;
using Microsoft.Extensions.Options;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email не может быть пустым")
            .EmailAddress().WithMessage("Неверный Email или пароль");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Пароль не может быть пустым")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов")
            .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches(@"[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.");
    }
}