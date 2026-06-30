using MediatR;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Features.AppRole.Commands.Delete
{
    // Command
    public sealed record DeleteAppRoleCommand(string Id) : IRequest<Result>;
}