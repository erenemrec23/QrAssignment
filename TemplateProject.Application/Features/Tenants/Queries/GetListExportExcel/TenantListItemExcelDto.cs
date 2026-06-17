using QrAssignment.Application.Attributes;
using QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel;

namespace QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel
{
    public class TenantListItemExcelDto
    { 
        [ColumnDisplay(1)]
        public string Name { get; set; }
         
         
    }
}

