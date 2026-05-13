using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TemplateProject.Application.Interfaces;
using TemplateProject.Domain.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Persistance.Interceptors;

public sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public AuditInterceptor(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
         
        var currentUserId = _userContext.GetCurrentUserId();
        var currentTime = DateTimeOffset.UtcNow;
         
        var entries = dbContext.ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.IsDeleted = false;
                entry.Entity.CreatedByUserId = currentUserId;
                entry.Entity.CreatedDate = currentTime;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedByUserId = currentUserId;
                entry.Entity.ModifiedDate = currentTime;
            }
            else if (entry.State == EntityState.Deleted && entry.Entity.IsDeleted == false)
            {
                entry.Entity.IsDeleted = true;
                entry.Entity.ModifiedByUserId = currentUserId;
                entry.Entity.ModifiedDate = currentTime;
            } 
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}