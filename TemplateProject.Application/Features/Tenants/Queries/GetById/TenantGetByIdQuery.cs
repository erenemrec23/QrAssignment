using MediatR;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class TenantGetByIdQuery : IRequest<Result<List<TenantItemDto>>>
    {
        public Guid Id { get; set; }

        public TenantGetByIdQuery(Guid id)
        {
            Id = id;
        }
    } 
}
