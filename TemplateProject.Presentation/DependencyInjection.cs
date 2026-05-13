using Microsoft.Extensions.DependencyInjection;
using TemplateProject.Presentation.Middlewares; // Handler'ın olduğu klasör

namespace TemplateProject.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    { 
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(); // JSON hata formatı için şart

        return services;
    }
}