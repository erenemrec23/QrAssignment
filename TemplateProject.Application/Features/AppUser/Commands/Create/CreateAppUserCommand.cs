using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Commands.Create
{
    public sealed record CreateAppUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Result<Unit>>;
}
