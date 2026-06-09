using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Entity.Audit;
using QrAssignment.Domain.Entity.System;
using System.Linq.Expressions;
using System.Reflection;

namespace QrAssignment.Persistance.Context;

public class AppDbContext : AuditDbContext
{
    private readonly ITenantService _tenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<SystemAuditLog> SystemAuditLogs { get; set; }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppUserRole> AppUserRole { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRefreshToken> AppUserRefreshTokens { get; set; }

      
    //public DbSet<QrApplicant> QrApplicants { get; set; }

    public DbSet<QrLocation> QrLocations { get; set; }

    public DbSet<SystemRegion> SystemRegions { get; set; }


    public DbSet<Tenant> Tenants { get; set; }

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
                var filter = ConvertFilterExpressionOfIsDeleted(entityType.ClrType);
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

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpressionOfIsDeleted(entityType.ClrType));
        }


        var tenantEntityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(IMustHaveTenant).IsAssignableFrom(e.ClrType) && e.ClrType.IsClass);

        // 2. Her bir tablo için dinamik Query Filter metodumuzu çalıştır
        foreach (var entityType in tenantEntityTypes)
        {
            var method = typeof(AppDbContext)
                .GetMethod(nameof(SetTenantQueryFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(entityType.ClrType);

            method?.Invoke(this, new object[] { modelBuilder });
        }

    }

    private static LambdaExpression ConvertFilterExpressionOfIsDeleted(Type entityType)
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
    private void SetTenantQueryFilter<TEntity>(ModelBuilder builder)
       where TEntity : class, IMustHaveTenant
    {
        // KRİTİK NOKTA: tenantId'yi dışarıda değişkene atamıyoruz! 
        // İfadeyi doğrudan this._tenantService üzerine kuruyoruz ki EF Core bunu anlık okusun.
        builder.Entity<TEntity>().HasQueryFilter(x => x.TenantId == _tenantService.GetTenantId());
    }

   
}