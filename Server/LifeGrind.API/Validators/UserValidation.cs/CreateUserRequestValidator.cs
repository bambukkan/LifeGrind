using FluentValidation;
using Microsoft.Extensions.Options;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Имя не может быть пустым")
            .MinimumLength(2).WithMessage("Имя должно состоять минимум из 2 символов")
            .MaximumLength(80).WithMessage("Имя не может превышать 80 символов");    
        
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