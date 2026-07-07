using MediatR; 
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Create
{
    // Command
    public sealed record CreateRoleCommand(string Name) : IRequest<Result>;
}