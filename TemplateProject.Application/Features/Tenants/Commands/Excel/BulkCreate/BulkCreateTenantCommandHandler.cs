using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate
{
    public class BulkCreateTenantCommandHandler : IRequestHandler<BulkCreateTenantCommand, Result<List<Guid>>>
    {
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;

        public BulkCreateTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<List<Guid>>> Handle(BulkCreateTenantCommand request, CancellationToken cancellationToken)
        {
            if (request.Tenants == null || !request.Tenants.Any())
            {
                return Result.Failure<List<Guid>>(new Error("Yüklenecek geçerli bir veri bulunamadı.", "TENANT_BULK_CREATE_NO_DATA"));
            }
             
            var tenantList = _mapper.Map<List<Tenant>>(request.Tenants);
             
            await _tenantRepository.AddRangeAsync(tenantList, cancellationToken);
             
            var createdIds = tenantList.Select(t => t.Id).ToList();

            return Result.Success(createdIds);
        }
    }
}