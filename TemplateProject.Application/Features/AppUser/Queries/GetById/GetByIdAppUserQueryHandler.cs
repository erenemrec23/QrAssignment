using MediatR;
using QrAssignment.Application.Features.AppUser.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Queries.GetById
{
    public class GetByIdAppUserQueryHandler : IRequestHandler<GetByIdAppUserQuery, Result<AppUserItemDto>>
    {
        private readonly IAppUserRepository _appUserRepository;
    

        public GetByIdAppUserQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<AppUserItemDto>> Handle(GetByIdAppUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetById(request.Id, cancellationToken);

            return Result.Success(result);
        }
    }
}
