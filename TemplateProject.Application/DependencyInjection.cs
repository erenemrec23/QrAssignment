using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Behaviors;
using System.Reflection;
namespace QrAssignment.Application
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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(SharedResource).Assembly);
            return services;
        }
    }
}
