using MediatR;
using QrAssignment.Application.Features.Tenants.Queries.GetById;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
    
    public class PermissionUserGetByUserIdQuery : IRequest<Result<PermissionUserItemDto>>, ISecuredRequest
    {

        public string PageName => "Page_Users";
        public PagePermissions RequiredPermission => PagePermissions.View;
        public Guid? UserId { get; set; }

        public PermissionUserGetByUserIdQuery(Guid? userId)
        {
            UserId = userId;
        }
    }
}
