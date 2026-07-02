using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppRole.Commands.Delete
{
    // Command
    public sealed record DeleteAppRoleCommand(string Id) : IRequest<Result>;
}