using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Commands.CreateAppUser
{
    public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Result<Unit>>; // Değer dönmeyeceksek ICommand yeterli
}
