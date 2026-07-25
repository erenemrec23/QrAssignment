using QrAssignment.Application.Attributes;

namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantListItemExcelDto
    {
        [ColumnDisplay(1)]
        public string Code { get; set; }
        [ColumnDisplay(2)]
        public string Name { get; set; }
         
         
    }
}

