using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Behaviors;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using System.Reflection;
namespace QrAssignment.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {  
            services.AddScoped<IExcelRowBusinessValidator<BulkCreateTenantInputDto>, BulkCreateTenantNameUniquenessValidator>();
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
