using MediatR;
using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.AuthFeatures.Commands.Login
{
    public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : ICommand<Result<LoginCommandResponse>>;
}
