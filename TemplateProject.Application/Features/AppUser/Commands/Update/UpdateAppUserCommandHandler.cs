using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Commands.Update
{
    internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateAppUserCommand, Result<Unit>>
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppLocalizer _localizer;
        public UpdateUserCommandHandler(IAppUserRepository appUserRepository, IUserRepository userRepository, IAppLocalizer localizer)
        {
            _appUserRepository = appUserRepository;
            _userRepository = userRepository;
            _localizer = localizer;
        }

        public async Task<Result<Unit>> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _appUserRepository.GetByIdWithRefreshTokenAsync(request.Id, cancellationToken);

            if (user is null)
                throw new Exception(_localizer["Messages.UserNotFound"]);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            // Sadece update metodunu çağırıyoruz, gerisini Pipeline Behavior halledecek.
            _userRepository.Update(user);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
