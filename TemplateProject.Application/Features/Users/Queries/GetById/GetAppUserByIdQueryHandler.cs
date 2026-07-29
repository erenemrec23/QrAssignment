using MediatR;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.GetById
{
    public class GetAppUserByIdQueryHandler : IRequestHandler<GetAppUserByIdQuery, Result<AppUserItemDto>>
    {
        private readonly IAppUserRepository _appUserRepository;
    

        public GetAppUserByIdQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<AppUserItemDto>> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetById(request.Id, cancellationToken);

            return Result.Success(result);
        }
    }
}
