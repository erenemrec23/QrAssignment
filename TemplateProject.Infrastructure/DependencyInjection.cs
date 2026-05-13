using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Configuration;
using TemplateProject.Application.Interfaces;
using TemplateProject.Infrastructure.Authentication;
using TemplateProject.Infrastructure.Localization;
using TemplateProject.Infrastructure.Services;

namespace TemplateProject.Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // typeof(DependencyInjection) yerine .Assembly ekliyoruz
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<IJwtProvider, JwtProvider>(); 

            return services;
        }
    }
}
