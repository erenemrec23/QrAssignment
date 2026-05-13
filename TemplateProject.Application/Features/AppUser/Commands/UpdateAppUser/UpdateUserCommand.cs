using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.AppUser.Commands.UpdateAppUser
{
    public sealed record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName) : ICommand<Result<Unit>>;
}
