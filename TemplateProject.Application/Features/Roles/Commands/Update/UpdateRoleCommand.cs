using MediatR;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.Roles.Commands.Update
{
    // Command
    public sealed record UpdateRoleCommand(string Id, string Name) : IRequest<Result>;
}