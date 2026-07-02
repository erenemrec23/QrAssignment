using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Commands.CreateAppUser
{
    public sealed record CreateAppUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Result<Unit>>, ISecuredRequest
    {
        public string PageName => "Page_AppUsers";
        public PagePermissions RequiredPermission => PagePermissions.Insert;
    };
}
