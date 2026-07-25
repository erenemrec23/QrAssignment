using MediatR;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.GetById
{
    public sealed class GetByIdRoleQueryHandler : IRequestHandler<GetByIdRoleQuery, Result<RoleListItemDto>>
    {
        private readonly IAppRoleRepository _appRoleRepository;

        public GetByIdRoleQueryHandler(IAppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result<RoleListItemDto>> Handle(GetByIdRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetDtoByIdAsync(request.Id.Value, cancellationToken);

            return Result.Success(result);
        }
    }
}