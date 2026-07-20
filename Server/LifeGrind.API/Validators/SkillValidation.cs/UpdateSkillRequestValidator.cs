using FluentValidation;
using Microsoft.Extensions.Options;

public class UpdateSkillRequestValidator : AbstractValidator<UpdateSkillRequest>
{
    public UpdateSkillRequestValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MinimumLength(2).WithMessage("Название должно состоять минимум из 2 символов")
            .MaximumLength(80).WithMessage("Название не может превышать 80 символов");    
        
        RuleFor(s => s.Description)
            .NotNull().WithMessage("Описание обязано быть в запросе.");
        
        RuleFor(s => s.Experience)
            .NotNull().WithMessage("Опыт обязан быть в запросе")
            .GreaterThanOrEqualTo(0).WithMessage("Опыт не может быть меньше нуля");
    }
}