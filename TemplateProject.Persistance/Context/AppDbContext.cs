using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Entity.Audit;
using QrAssignment.Domain.Entity.System;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Context;

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
    //public DbSet<QrApplicant> QrApplicants { get; set; }

    public DbSet<QrLocation> QrLocations { get; set; }

    public DbSet<SystemRegion> SystemRegions { get; set; }
     


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