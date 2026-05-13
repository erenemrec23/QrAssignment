using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity.App;
using TemplateProject.Persistance; // AppDbContext'in olduğu yer

namespace TemplateProject.Persistance.Repositories;

public sealed class AppUserRepository : IAppUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public AppUserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken cancellationToken = default)
    { 
        return await _userManager.Users
            .Include(u => u.RefreshToken) 
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
    public async Task<AppUser?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users
            .Include(u => u.RefreshToken)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}