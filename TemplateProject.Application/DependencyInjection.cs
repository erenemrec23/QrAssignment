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

                // Sıralama önemli: liste yukarıdan aşağıya, en dıştan en içe doğru sarılır.
                cfg.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>)); // 1. En dış: hiçbir exception dışarı sızmasın
                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));     // 2. Yetki kontrolü, handler'a hiç girmesin
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));        // 3. FluentValidation kontrolü
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));        // 4. En iç: handler'a bitişik, SaveChanges burada
            });

            services.AddValidatorsFromAssembly(typeof(SharedResource).Assembly);
            services.AddTransient(typeof(IValidator<>), typeof(GetByIdQueryValidator<>));

            return services;
        }
    }
}
