using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace QrAssignment.Persistance.Repositories;

public sealed class AppRoleRepository : IAppRoleRepository
{
    private readonly RoleManager<AppRole> _roleManager;

    public AppRoleRepository(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<Paginate<AppRoleListItemDto>> GetListAsync(PageRequestBaseDto request, CancellationToken cancellationToken = default)
    {
        // RoleManager.Roles bize doğrudan IQueryable<AppRole> döner.
        IQueryable<AppRole> query = _roleManager.Roles;

        return await GetPaginatedRolesAsync(request,
            cancellationToken);
    }

    public async Task<Paginate<AppRoleListItemDto>> GetPaginatedRolesAsync(
        PageRequestBaseDto request,
        CancellationToken cancellationToken = default)
    {
        // 1. RoleManager'dan IQueryable sorgusunu al
        var query = _roleManager.Roles;

        // 2. Filtresiz toplam kayıt sayısı
        int totalItemCount = await query.CountAsync(cancellationToken);

        // 3. Global Search Uygulaması (Örn: Sadece Rol Adı içinde arama)
        if (request.GlobalSearch != null && !string.IsNullOrWhiteSpace(request.GlobalSearch.Value))
        {
            var searchTerm = request.GlobalSearch.Value.ToLower();
            // Sadece Name alanında arama yapıyoruz
            query = query.Where(r => r.Name.ToLower().Contains(searchTerm));
        }

        // İSTEĞE BAĞLI: Eğer AppRole için de Dynamic LINQ (Sıralama vb.) kullanmak istersen
        // if (request.DynamicFilterAndSort != null)
        // {
        //     query = query.ToDynamic(request.DynamicFilterAndSort);
        // }

        // 4. Filtre uygulandıktan sonraki toplam kayıt sayısı
        int totalFilteredItemCount = await query.CountAsync(cancellationToken);

        // 5. Sayfalama (Skip & Take) ve DTO'ya Dönüştürme
        var items = await query
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new AppRoleListItemDto(r.Id.ToString(), r.Name!))
            .ToListAsync(cancellationToken);

        // 6. Senin standart Paginate nesneni döndür
        return new Paginate<AppRoleListItemDto>
        {
            Index = request.PageIndex,
            PageSize = request.PageSize,
            TotalItemCount = totalItemCount,
            TotalFilteredItemCount = totalFilteredItemCount,
            Items = items
        };
    }
}
