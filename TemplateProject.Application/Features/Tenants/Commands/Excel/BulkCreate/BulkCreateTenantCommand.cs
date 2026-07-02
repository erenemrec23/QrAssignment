using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate
{
    // Geriye toplu eklenen Tenant ID listesini dönebiliriz
    public class BulkCreateTenantCommand : ICommand<Result<List<Guid>>>
    {

        public List<CreateTenantInputDto> Tenants { get; set; } = new();
    }

    // Excel satırlarından map'lenecek hafif bir DTO
    public class CreateTenantInputDto
    {
        public string Name { get; set; } 
    }
}