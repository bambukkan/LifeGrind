using FluentValidation;

public class UpdatePersonalRewardRequestValidator : AbstractValidator<UpdatePersonalRewardRequest>
{
    public UpdatePersonalRewardRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Название награды не должно быть пустым")
            .MinimumLength(2).WithMessage("Слишком короткое название награды")
            .MaximumLength(40).WithMessage("Слишком длинное название награды");
        RuleFor(p => p.Description)
            .NotNull().WithMessage("Описание награды должно быть в запросе")
            .MaximumLength(250).WithMessage("Слишком длинное описание награды");    

        RuleFor(p => p.Cost)
            .NotEmpty().WithMessage("Стоимость награды не должна быть пустой")
            .GreaterThan(0).WithMessage("Цена награды не может быть меньше 1");
    }
}