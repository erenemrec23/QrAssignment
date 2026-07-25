using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate
{
    // Geriye toplu eklenen Tenant ID listesini dönebiliriz
    public class BulkCreateTenantCommand : ICommand<Result<List<Guid>>>
    {

        public List<BulkCreateTenantInputDto> Tenants { get; set; } = new();
    }
}