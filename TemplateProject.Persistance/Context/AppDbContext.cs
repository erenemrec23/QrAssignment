using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TemplateProject.Domain.Abstractions; 
using TemplateProject.Domain.Entities;
using TemplateProject.Domain.Entity;
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Persistence.Context;

public class AppDbContext : AuditDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SystemAuditLog> SystemAuditLogs { get; set; }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppUserRole> AppUserRole { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRefreshToken> AppUserRefreshTokens { get; set; }

     
    public DbSet<Car> Cars { get; set; }
    public DbSet<Brand> Brands { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<IdentityUserLogin<string>>();
        modelBuilder.Ignore<IdentityUserRole<string>>();
        modelBuilder.Ignore<IdentityUserClaim<string>>();
        modelBuilder.Ignore<IdentityUserToken<string>>();
        modelBuilder.Ignore<IdentityRoleClaim<string>>();
        modelBuilder.Ignore<IdentityRole<string>>();

        modelBuilder.Entity<AppUser>(b =>
        {

            b.Property<byte[]>("RowVersion")  
                .IsRowVersion();

        });
         
        modelBuilder.Entity<AppRole>(b =>
        {
            b.Property<byte[]>("RowVersion") 
                .IsRowVersion();
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        { 
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(BaseEntity))
            { 
                var filter = ConvertFilterExpression(entityType.ClrType);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }


        var softDeleteEntities = modelBuilder.Model.GetEntityTypes()
       .Where(e => e.ClrType != typeof(BaseEntity) && e.BaseType == null &&
                    typeof(BaseEntity).IsAssignableFrom(e.ClrType) ||
                   e.ClrType == typeof(AppUser) ||
                   e.ClrType == typeof(AppRole));
        foreach (var entityType in softDeleteEntities)
        {

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
        }


    }

    private static LambdaExpression ConvertFilterExpression(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "p");
        var propertyAccess = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
         
        Expression falseConstant = Expression.Constant(false);
         
        if (propertyAccess.Type != typeof(bool))
        {
            falseConstant = Expression.Convert(falseConstant, propertyAccess.Type);
        }
         
        var equalExpression = Expression.Equal(propertyAccess, falseConstant);

        return Expression.Lambda(equalExpression, parameter);
    }  
}