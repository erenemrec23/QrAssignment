using MediatR;
using QrAssignment.Application.Features.Tenants.Queries.GetById;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

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
