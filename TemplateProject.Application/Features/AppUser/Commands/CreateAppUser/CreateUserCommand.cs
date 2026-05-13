using MediatR;
using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.AppUser.Commands.CreateAppUser
{
    public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Result<Unit>>; // Değer dönmeyeceksek ICommand yeterli
}
