using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using System.Windows.Input;

namespace QrAssignment.Application.Features.Roles.Commands.Create
{
    // Command
    public sealed record CreateRoleCommand(string Name) : ICommand<Result>;
}