using MediatR;
using Microsoft.Extensions.Localization;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Helpers;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel
{
    public class GetListTenantExportExcelQueryHandler : IRequestHandler<GetListTenantExportExcelQuery, Result<ExcelFileDto>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IStringLocalizer<GetListTenantExportExcelQueryHandler> _localizer;

        public GetListTenantExportExcelQueryHandler(
            ITenantRepository tenantRepository,
            IStringLocalizer<GetListTenantExportExcelQueryHandler> localizer)
        {
            _tenantRepository = tenantRepository;
            _localizer = localizer;
        }

        public async Task<Result<ExcelFileDto>> Handle(GetListTenantExportExcelQuery request, CancellationToken cancellationToken)
        {

            var dataList = await _tenantRepository.GetExportListAsync(request, cancellationToken);

            // 2. Helper her şeyi reflection ve çeviri servisi ile kendisi hallediyor
            byte[] excelBytes = ExcelExportHelper.GenerateExcel(dataList, "Firmalar", _localizer);

            // 3. Dosyayı dön
            var resultDto = new ExcelFileDto
            {
                Data = excelBytes,
                FileName = $"Firmalar_Listesi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            return Result.Success(resultDto);
        }
        
    }
}
