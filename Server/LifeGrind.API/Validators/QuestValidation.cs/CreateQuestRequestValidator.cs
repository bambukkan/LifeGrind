using FluentValidation;
using Microsoft.Extensions.Options;

public class CreateQuestRequestValidator : AbstractValidator<CreateQuestRequest>
{
    public CreateQuestRequestValidator()
    {
        RuleFor(q => q.Title)
            .NotEmpty().WithMessage("Заголовок не должен быть пустым.")
            .MinimumLength(2).WithMessage("Заголовок должен содержать минимум 2 символа")
            .MaximumLength(150).WithMessage("Заголовок не может содержать больше 150 символов");        
        RuleFor(q => q.Description)
            .NotNull().WithMessage("Описание обязано быть в запросе.");
        RuleFor(q => q.Difficulty)
            .IsInEnum().WithMessage("Указана недопустимая сложность квеста");
        RuleFor(q => q.ExperienceReward)
            .NotEmpty().WithMessage("Опыт за квест не должен быть пустым.")
            .GreaterThan(0).WithMessage("Опыт за квест не может быть меньше или равен нуля");
        RuleFor(q => q.CoinReward)
            .NotEmpty().WithMessage("Монеты за квест не должен быть пустым.")
            .GreaterThan(0).WithMessage("Монеты за квест не может быть меньше или равен нуля"); 
    }
}