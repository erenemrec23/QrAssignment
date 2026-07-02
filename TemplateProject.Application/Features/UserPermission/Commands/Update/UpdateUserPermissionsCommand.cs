using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record UpdateUserPermissionsCommand(
        string UserId,
        List<PermissionUserUpdateDto> Permissions) : IRequest<Result>, ISecuredRequest
    {

        public string PageName => "Page_Users";
        public PagePermissions RequiredPermission => PagePermissions.Update;
    };
}
