using MediatR; 
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.GetAssignedPermissionList
{
    public class GetAssignedPermissionListQuery : IRequest<Result<RolePermissionDto>>
    {
        public GetAssignedPermissionListQuery() { }
        public GetAssignedPermissionListQuery(Guid? roleId)
        {
            RoleId = roleId;
        }

        public Guid? RoleId { get; set; }
    }
}