using Microsoft.AspNetCore.Identity;
using TemplateProject.Application.Features.AuthFeatures.Commands.Login;
using TemplateProject.Application.Interfaces;
using TemplateProject.Application.Repositories;
using TemplateProject.Application.Services;
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Persistance.Services
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
            IAppLocalizer localizer)
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
                throw new Exception(_localizer["Messages.UserMailUserPasswordNotFound"]); 
            } 
            bool checkPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!checkPassword)
            {
                throw new Exception(_localizer["Messages.UserMailUserPasswordNotFound"]);
            } 
            var token = await _jwtProvider.CreateTokenAsync(user);

            return token;
        }


        public async Task CreateAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken)
        { 
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null)
            {
                throw new Exception(_localizer["Messages.MailAlreadyExists"]);
            }
             
            AppUser user = new()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email  
            };
             
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            { 
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(error);
            }
        }
    }
}
