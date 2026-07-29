using MediatR;
using QrAssignment.Application.Features.QrLocations.Queries.GetList;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Users.Queries.GetList
{
    public class GetUserListQueryHandler  : IRequestHandler<GetUserListQuery, Result<List<AppUserListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetUserListQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<List<AppUserListItemDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetList(cancellationToken);

            return Result.Success(result);
        }
    }
}
