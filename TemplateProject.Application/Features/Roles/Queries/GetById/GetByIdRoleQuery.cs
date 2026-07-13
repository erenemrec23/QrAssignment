using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.GetById
{
    public sealed class GetByIdRoleQuery : IRequest<Result<RoleListItemDto>> , IdValidationBase
    {
        public GetByIdRoleQuery(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
    }
}