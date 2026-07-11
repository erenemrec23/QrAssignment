using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class GetByIdTenantQuery : ICommand<Result<TenantItemDto>>, IIdQuery
    {
        public Guid? Id { get; set; }

        public GetByIdTenantQuery(Guid? id)
        {
            Id = id;
        }
    } 
}
