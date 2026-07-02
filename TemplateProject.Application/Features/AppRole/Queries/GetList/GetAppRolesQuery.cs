using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppRole.Queries.GetList
{

    // Query
    // Result nesnesinin generic versiyonu (Result<T>) ile data dönüyoruz
    public sealed record GetAppRolesQuery() : IRequest<Result<List<AppRoleListItemDto>>>, ISecuredRequest
    {
        public string PageName => "Page_AppRoles";
        public PagePermissions RequiredPermission => PagePermissions.View;
    };
}