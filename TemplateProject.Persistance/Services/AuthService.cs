using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Exceptions;

namespace QrAssignment.Persistance.Services
{
    internal sealed class AuthService : IAuthService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IAppLocalizer _localizer; 

        public AuthService(IAppUserRepository userRepository,
            UserManager<AppUser> userManager,
            IJwtProvider jwtProvider,
            IAppLocalizer localizer, 
            ITenantService tenantService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _jwtProvider = jwtProvider; 
            _localizer = localizer; 
        }

        public async Task<LoginCommandResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
        { 
            AppUser? user = await _userRepository.GetByEmailWithRefreshTokenAsync(email, cancellationToken);
          
            if (user is null)
            {
                throw new BusinessException(_localizer["Messages.UserMailUserPasswordNotFound"]); 
            } 
            bool checkPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!checkPassword)
            {
                throw new BusinessException(_localizer["Messages.UserMailUserPasswordNotFound"]);
            } 
            var token = await _jwtProvider.CreateTokenAsync(user);

            return token;
        }


        public async Task CreateAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken)
        { 
            var existingUser = await _userRepository.GetByEmailWithRefreshTokenAsync(email);
            if (existingUser is not null)
            {
                throw new BusinessException(_localizer["Messages.MailAlreadyExists"]);
            }
             
            AppUser user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email, 
            };
             
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            { 
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException(error);
            }
        }
    }
}
