using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.AppRole.Queries.GetList
{
    // Handler
    public sealed class GetAppRolesQueryHandler : IRequestHandler<GetAppRolesQuery, Result<Paginate<AppRoleListItemDto>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;

        public GetAppRolesQueryHandler(IAppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result<Paginate<AppRoleListItemDto>>> Handle(GetAppRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetListAsync(request, cancellationToken);
             
            return Result.Success(result);
        }
    }
}