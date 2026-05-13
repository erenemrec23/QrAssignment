using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TemplateProject.Application.Behaviors;  
namespace TemplateProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // typeof(DependencyInjection) yerine .Assembly ekliyoruz
            services.AddAutoMapper(cfg => { }, typeof(DependencyInjection));

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
            return services;
        }
    }
}
