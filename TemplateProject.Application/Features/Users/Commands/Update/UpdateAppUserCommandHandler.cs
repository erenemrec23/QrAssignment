using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;   // AppUser
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    internal sealed class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppLocalizer _localizer;

        public UpdateAppUserCommandHandler(UserManager<AppUser> userManager, IAppLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user is null)
                return Result.Failure(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result.Failure(new Error(
                    "Error.UserCanNotUpdated",
                    string.Format(
                        _localizer["Error.UserCanNotUpdated"],
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)))));

            return Result.Success();
        }
    }
}
