using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName) : ICommand<Result<Unit>>;
}
