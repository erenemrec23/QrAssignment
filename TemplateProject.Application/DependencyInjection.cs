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
            services.AddAutoMapper(cfg => { }, typeof(DependencyInjection));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                 
                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));     
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));        
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));        
            });

            services.AddValidatorsFromAssembly(typeof(SharedResource).Assembly);
            services.AddTransient(typeof(IValidator<>), typeof(GetByIdQueryValidator<>));

            return services;
        }
    }
}
