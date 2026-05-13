using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Interfaces;
using QrAssignment.Infrastructure.Authentication;
using QrAssignment.Infrastructure.Services;

namespace QrAssignment.Infrastructure
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
