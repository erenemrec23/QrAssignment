using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record UpdateUserPermissionCommand(
        string UserId,
        List<PermissionUserUpdateDto> Permissions) : ICommand<Result>
    {

    };
}
