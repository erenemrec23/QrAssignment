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
            var resultIdList = new List<Guid>();
            var codeIsNullList = request.Tenants.Where(w => !w.Code.HasValue);
            if (codeIsNullList.Any())
            {
                var tenantList = _mapper.Map<List<Tenant>>(request.Tenants);

                await _tenantRepository.AddRangeAsync(tenantList, cancellationToken);
                resultIdList.AddRange(tenantList.Select(t => t.Id).ToList());
            }
            var codeIsNotNullList = request.Tenants.Where(w => w.Code.HasValue).Select(s=>s.Code.Value).ToList();
            var updateList = _tenantRepository.GetByRevNumsAsync(codeIsNotNullList, cancellationToken);

            var resultHasNoUpdateData = new List<long>();
            foreach (var code in codeIsNotNullList)
            {
                var dataUpdate = updateList.Result.SingleOrDefault(s => s.RevNum == code);
                var requestDto = request.Tenants.Single(w => w.Code == code);
                if (dataUpdate != null)
                { 
                    _mapper.Map(request, dataUpdate);
                    _tenantRepository.Update(dataUpdate);
                    resultIdList.Add(dataUpdate.Id);
                }
                else
                {
                    resultHasNoUpdateData.Add(code);
                } 
            }
            if (resultHasNoUpdateData.Any())
            {
                Result.Failure(new Error("HasNoUpdateData", string.Format("Girmiş Olduğunuz Kod(lar)a Ship Bir Data Bulunamadı ({0})",string.Join(", ", resultHasNoUpdateData))));
            }

            return Result.Success(resultIdList);
        }
    }
}