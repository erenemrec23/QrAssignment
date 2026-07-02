using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppRole.Queries.GetList
{

    // Query
    // Result nesnesinin generic versiyonu (Result<T>) ile data dönüyoruz
    public sealed class GetAppRolesQuery : PageRequestBaseDto, IRequest<Result<Paginate<AppRoleListItemDto>>>;
}