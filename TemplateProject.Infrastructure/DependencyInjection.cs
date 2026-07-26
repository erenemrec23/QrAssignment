using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Infrastructure.Authentication;
using QrAssignment.Infrastructure.Excel;
using QrAssignment.Infrastructure.Services;

namespace QrAssignment.Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExcelSampleTemplateGenerator, ExcelSampleTemplateGenerator>();
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IUserContext, UserContext>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<IJwtProvider, JwtProvider>(); 

            return services;
        }
    }
}
