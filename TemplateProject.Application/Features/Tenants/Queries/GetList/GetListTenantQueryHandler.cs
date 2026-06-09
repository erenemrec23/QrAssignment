using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetList
{
    public class GetListTenantQueryHandler : IRequestHandler<GetListTenantQuery, Result<List<TenantListItemDto>>>
    {
        private readonly ITenantRepository _tenantRepository;

        public GetListTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<List<TenantListItemDto>>> Handle(GetListTenantQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetList(cancellationToken);

            return Result.Success(result);
        }

    }
}
