using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.AppUser.Queries.GetById;
using QrAssignment.Application.Features.AppUser.Queries.GetList; 
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App; 

namespace QrAssignment.Persistance.Repositories;

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


    public async Task<List<AppUserListItemDto>> GetList(CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .AsNoTracking()
            .Select(u => new AppUserListItemDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
            })
            .ToListAsync(cancellationToken);
    }


    public async Task<AppUserItemDto> GetById(Guid? id, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Where(w=> w.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user == null)
            return null;

        return new AppUserItemDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName };
    }
}