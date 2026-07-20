using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly ILogger<ValidationFilter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(ILogger<ValidationFilter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null)
            {
                continue;
            }

            var argType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argType);
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator == null)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);
            var validationResult = await validator.ValidateAsync(validationContext);

            if (validationResult.IsValid)
            {
                continue;
            }

            _logger.LogWarning("DTO validation failed for {DtoType}", argument.GetType().Name);

            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new { Errors = errors });
            return;
        }

        await next();
    }
}
