using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.Roles.Queries.GetList
{
    // Handler
    public sealed class GetRoleQueryHandler : IRequestHandler<GetRoleListQuery, Result<Paginate<RoleListItemDto>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;

        public GetRoleQueryHandler(IAppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result<Paginate<RoleListItemDto>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetListAsync(request, cancellationToken);
             
            return Result.Success(result);
        }
    }
}