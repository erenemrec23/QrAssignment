using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class GetTenantByIdQuery : ICommand<Result<TenantItemDto>>
    {
        public Guid Id { get; set; }

        public GetTenantByIdQuery(Guid id)
        {
            Id = id;
        }
    } 
}
