using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Create
{
    public sealed record CreateAppUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : ICommand<Result>;
}
