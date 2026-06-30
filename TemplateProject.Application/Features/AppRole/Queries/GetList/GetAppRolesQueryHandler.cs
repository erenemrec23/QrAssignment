using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.AppRole.Queries.GetList
{
    // Handler
    public sealed class GetAppRolesQueryHandler : IRequestHandler<GetAppRolesQuery, Result<List<AppRoleListItemDto>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;

        public GetAppRolesQueryHandler(IAppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result<List<AppRoleListItemDto>>> Handle(GetAppRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetList(cancellationToken);
             
            return Result.Success(result);
        }
    }
}