using MediatR;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class GetByIdTenantQueryHandler : IRequestHandler<TenantGetByIdQuery, Result<List<TenantItemDto>>>
    {
        private readonly ITenantRepository _tenantRepository;

        public GetByIdTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<List<TenantItemDto>>> Handle(TenantGetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetById(request.Id, cancellationToken);

            return Result.Success(result);
        }
    } 
}
