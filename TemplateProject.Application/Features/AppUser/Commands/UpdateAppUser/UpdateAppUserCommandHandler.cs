using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.AppUser.Commands.UpdateAppUser
{
    internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateAppUserCommand, Result<Unit>>
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IUserRepository _userRepository;  
        public UpdateUserCommandHandler(IAppUserRepository appUserRepository, IUserRepository userRepository)
        {
            _appUserRepository = appUserRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<Unit>> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _appUserRepository.GetByIdWithRefreshTokenAsync(request.Id, cancellationToken);

            if (user is null)
                throw new Exception("Kullanıcı bulunamadı!");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            // Sadece update metodunu çağırıyoruz, gerisini Pipeline Behavior halledecek.
            _userRepository.Update(user);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
