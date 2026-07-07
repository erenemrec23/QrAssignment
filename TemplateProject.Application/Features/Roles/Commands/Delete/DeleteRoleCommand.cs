using MediatR; 
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Delete
{
    // Command
    public sealed record DeleteRoleCommand(string Id) : IRequest<Result>;
}