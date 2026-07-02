using MediatR;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.AppRole.Commands.Update
{
    // Command
    public sealed record UpdateAppRoleCommand(string Id, string Name) : IRequest<Result>;
}