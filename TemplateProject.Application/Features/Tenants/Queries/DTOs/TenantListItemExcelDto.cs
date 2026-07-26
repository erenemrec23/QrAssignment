using QrAssignment.Application.Attributes;
using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantListItemExcelDto
    {
        [ExcelColumn("Tenant.Code", Order = 1)]
        public string Code { get; set; }

        [ExcelColumn("Tenant.Name", Order = 2)]
        public string Name { get; set; }
    }
}