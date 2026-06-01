using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Presentation.Middlewares;

namespace QrAssignment.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    { 
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();  

        return services;
    }
}