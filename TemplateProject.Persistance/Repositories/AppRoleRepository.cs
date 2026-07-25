using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using System.Linq.Expressions;

namespace QrAssignment.Persistance.Repositories;

internal sealed class AppRoleRepository : GenericAppRepository<AppRole>, IAppRoleRepository
{
    public AppRoleRepository(AppDbContext context) : base(context) { }

    private static Expression<Func<AppRole, RoleListItemDto>> Projection =>
        r => new RoleListItemDto(r.Id, r.Name!);

    public Task<Paginate<RoleListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginateAsync(Projection, request, ct);

    public Task<Paginate<RoleListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => PaginatePassivedAsync(Projection, request, ct);

    public Task<List<RoleListItemDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default)
        => ListAsync(Projection, request, ct);

    public Task<RoleListItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SingleDtoByIdAsync(id, Projection, ct);

    public Task<RoleListItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default)
        => SinglePassivedDtoByIdAsync(id, Projection, ct);

    public Task BulkDelete(List<Guid> ids, CancellationToken ct)
        => RemoveByIdsAsync(ids, ct);
}