using Audit.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TemplateProject.Application.Abstractions;
using TemplateProject.Application.Repositories;
using TemplateProject.Application.Services;
using TemplateProject.Domain.Entities;
using TemplateProject.Domain.Entity.App;
using TemplateProject.Persistance.Interceptors;
using TemplateProject.Persistance.Repositories;
using TemplateProject.Persistance.Services;
using TemplateProject.Persistence.Context;
using TemplateProject.Persistence.Repositories;


namespace TemplateProject.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<AuditInterceptor>();
             
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                 
                options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddIdentity<AppUser, AppUserRole>(options =>
            { 
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                 
                options.User.RequireUniqueEmail = true;
            })
    .AddEntityFrameworkStores<AppDbContext>();
             
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>(); 
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            Audit.Core.Configuration.Setup()
    .UseEntityFramework(ef => ef
        .AuditTypeMapper(t => typeof(SystemAuditLog)) 
        .AuditEntityAction<SystemAuditLog>((ev, entry, auditEntity) =>
        {

            auditEntity.TableName = entry.Table;
            auditEntity.Action = entry.Action;
            auditEntity.PrimaryKey = JsonSerializer.Serialize(entry.PrimaryKey);

            auditEntity.ColumnValues = JsonSerializer.Serialize(entry.ColumnValues);
            if (entry.Action == "Insert")
            {
                auditEntity.OldValues = null; 
                auditEntity.NewValues = JsonSerializer.Serialize(entry.ColumnValues);
            } 
            else if (entry.Action == "Update")
            { 
                auditEntity.OldValues = entry.Changes == null ? null :
                    JsonSerializer.Serialize(entry.Changes.ToDictionary(c => c.ColumnName, c => c.OriginalValue));

                auditEntity.NewValues = entry.Changes == null ? null :
                    JsonSerializer.Serialize(entry.Changes.ToDictionary(c => c.ColumnName, c => c.NewValue));
            } 
            else if (entry.Action == "Delete")
            { 
                auditEntity.OldValues = JsonSerializer.Serialize(entry.ColumnValues);
                auditEntity.NewValues = null;
            }
        })
        .IgnoreMatchedProperties(true) 
    );

            return services;
        }

    }
}
