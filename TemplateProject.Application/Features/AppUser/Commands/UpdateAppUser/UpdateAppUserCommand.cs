using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Commands.UpdateAppUser
{
    public sealed record UpdateAppUserCommand(
    Guid Id,
    string FirstName,
    string LastName) : ICommand<Result<Unit>>, ISecuredRequest
    {
        public string PageName => "Page_AppUsers";
        public PagePermissions RequiredPermission => PagePermissions.Update;
    };
}
