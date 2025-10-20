namespace Pixelz.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddDefaultProblemDetails(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddProblemDetails(opt =>
        {
            opt.IncludeExceptionDetails = (_, _) => env.IsDevelopment();

            opt.Map<ValidationException>(ex => new()
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Extensions = { ["errors"] = ex.Errors }
            });

            opt.Map<InvalidOperationException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Invalid operation",
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message,
                Type = "https://httpstatuses.io/409"
            });
            opt.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            opt.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });

        return services;
    }
}
