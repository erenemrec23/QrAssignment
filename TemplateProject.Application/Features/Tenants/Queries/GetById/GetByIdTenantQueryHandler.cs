using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class GetByIdTenantQueryHandler : IRequestHandler<GetTenantByIdQuery, Result<TenantItemDto>>
    {


        private readonly ITenantRepository _tenantRepository;

        public GetByIdTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<TenantItemDto>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetById(request.Id, cancellationToken);

            return Result.Success(result);
        }
    } 
}
