using MediatR;
using QrAssignment.Application.Features.Tenants.Queries.GetById;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
     
    public class GetByIdPermissionUserQueryHandler : IRequestHandler<PermissionUserGetByUserIdQuery, Result<PermissionUserItemDto>>
    {
        private readonly IAppUserClaimRepository  _appUserClaimRepository;

        public GetByIdPermissionUserQueryHandler(IAppUserClaimRepository  appUserClaimRepository)
        {
            _appUserClaimRepository = appUserClaimRepository;
        }

        public async Task<Result<PermissionUserItemDto>> Handle(PermissionUserGetByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserClaimRepository.GetUserWithPermissionsAsync(request.UserId, cancellationToken);

            return Result.Success(new PermissionUserItemDto()
            {
                PagePermissionList = result,
                UserId = request.UserId
            });
        }
    }
}
