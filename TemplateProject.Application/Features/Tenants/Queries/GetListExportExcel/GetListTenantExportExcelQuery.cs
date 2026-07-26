using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel
{
    public class GetListTenantExportExcelQuery : PageRequestBaseDto, IRequest<Result<ExcelFileDto>>
    {

    }

     
}
