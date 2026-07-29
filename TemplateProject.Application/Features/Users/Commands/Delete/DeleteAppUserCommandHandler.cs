using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;   // AppUser
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Delete
{
    public sealed class DeleteAppUserCommandHandler : IRequestHandler<DeleteAppUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppLocalizer _localizer;

        public DeleteAppUserCommandHandler(UserManager<AppUser> userManager, IAppLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.Value.ToString());
            if (user is null)
                return Result.Failure(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Result.Failure(new Error(
                    "Error.UserCanNotDeleted",
                    string.Format(
                        _localizer["Error.UserCanNotDeleted"],
                        string.Join(", ", result.Errors.Select(e => e.Description)))));

            return Result.Success();
        }
    }
}
