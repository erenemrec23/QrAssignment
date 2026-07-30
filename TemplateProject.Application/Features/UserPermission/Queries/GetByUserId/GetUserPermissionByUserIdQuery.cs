using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
    
    public class GetUserPermissionByUserIdQuery : IRequest<Result<PermissionUserItemDto>>
    {

        public Guid? UserId { get; set; }

        public GetUserPermissionByUserIdQuery(Guid? userId)
        {
            UserId = userId;
        }
    }
}
