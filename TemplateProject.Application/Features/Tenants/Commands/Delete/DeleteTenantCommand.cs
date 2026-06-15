using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Delete
{
    public class DeleteTenantCommand : ICommand<Result<DeleteTenantResponse>>
    {
        public Guid? Id { get; set; }
    }
}