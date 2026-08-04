
using QrAssignment.Application.Features.Menu.Queries.DTOs;
namespace QrAssignment.Application.Repositories
{
    public interface IPageRepository
    {
        Task<List<PageCatalogItemDto>> GetCatalogAsync(CancellationToken ct = default);
        Task<List<MenuGroupDto>> GetMenuAsync(CancellationToken ct = default);
    }
}
 