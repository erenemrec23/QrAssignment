using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppRole.Commands.Create
{
    // Command
    public sealed record CreateAppRoleCommand(string Name) : IRequest<Result>;
}