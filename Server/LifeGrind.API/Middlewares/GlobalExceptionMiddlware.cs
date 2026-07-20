
using LifeGrind.CORE.Exceptions;

public class GlobalExceptionMiddlware
{
    private readonly RequestDelegate next;

    private readonly ILogger<GlobalExceptionMiddlware> logger;

    public GlobalExceptionMiddlware(RequestDelegate _next,
ILogger<GlobalExceptionMiddlware> _logger)
    {
        next = _next;
        logger = _logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(DomainException ex)
        {
            logger.LogWarning(ex, "Нарушение бизнес-правил: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(
                new
                {
                    Error = ex.GetType().Name,
                    Message = ex.Message      
                }
            );
        }
        catch (EntityNotFoundException ex)
        {
            logger.LogWarning(ex, "Нарушение бизнес-правил: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = ex.GetType().Name, 
                Message = ex.Message
            });
        }
        catch(Exception exception)
        {
            logger.LogError(exception,"Internal server error: {Message}", exception.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                Error = "Internal Server Error",
                Message = "Что то пошло не так" 
            });
        }
    }

}