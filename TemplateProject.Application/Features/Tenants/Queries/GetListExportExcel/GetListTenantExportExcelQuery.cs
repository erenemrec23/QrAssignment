using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel
{
    public class GetListTenantExportExcelQuery : PageRequestBaseDto, IRequest<Result<FileExportDto>>
    {
    }

    public class FileExportDto
    {
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
