using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Repositories
{
    public interface IAppRoleRepository 
    {
        Task<List<AppRoleListItemDto>> GetList(CancellationToken cancellationToken);
    }
}
 