using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Commands.UpdateAppUser
{
    public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName) : ICommand<Result<Unit>>;
}
