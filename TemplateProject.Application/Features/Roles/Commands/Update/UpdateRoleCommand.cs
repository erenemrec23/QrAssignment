using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using System.Windows.Input;

namespace QrAssignment.Application.Features.Roles.Commands.Update
{
    // Command
    public sealed record UpdateRoleCommand(Guid? Id, string Name) : ICommand<Result>, IdValidationBase;
}