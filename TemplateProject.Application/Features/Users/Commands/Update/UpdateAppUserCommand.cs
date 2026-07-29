using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    public sealed record UpdateAppUserCommand(
        Guid? Id,
        string FirstName,
        string LastName) : ICommand<Result>, IdValidationBase;
}
