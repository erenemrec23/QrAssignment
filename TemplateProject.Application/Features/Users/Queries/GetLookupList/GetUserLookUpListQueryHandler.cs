using MediatR;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Features.Users.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.GetLookupList
{
    public class GetUserLookUpListQueryHandler : IRequestHandler<GetUserLookUpListQuery, Result<List<AppUserLookUpListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetUserLookUpListQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<List<AppUserLookUpListItemDto>>> Handle(GetUserLookUpListQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetLookUpList(cancellationToken);

            return Result.Success(result);
        }

    }
}
