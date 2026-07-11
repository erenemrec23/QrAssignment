using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.GetList
{

    // Query
    // Result nesnesinin generic versiyonu (Result<T>) ile data dönüyoruz
    public sealed class GetListRoleQuery : PageRequestBaseDto, IRequest<Result<Paginate<RoleListItemDto>>>;
}