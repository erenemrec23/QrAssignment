using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Common;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Exceptions;
using QrAssignment.Domain.Shared;
using System.Text;

namespace QrAssignment.Persistance.Services
{
    internal sealed class AuthService : IAuthService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IAppLocalizer _localizer;
        private readonly IEmailService _emailService;
        private readonly IOptions<MailSettings> _mailSettings;
        private readonly ITenantIdService _tenantService;
        public AuthService(IAppUserRepository userRepository,
            UserManager<AppUser> userManager,
            IJwtProvider jwtProvider,
            IAppLocalizer localizer, 
            ITenantIdService tenantService,
            IEmailService emailService,
            IOptions<MailSettings> mailSettings )
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _jwtProvider = jwtProvider; 
            _localizer = localizer;
            _emailService = emailService;
            _mailSettings = mailSettings;
            _tenantService = tenantService;
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
                PasswordHash = _userManager.PasswordHasher.HashPassword(null, password),
            };
             
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            { 
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException(error);
            }
        }

        public async Task<Result> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailForRememberPasswordAsync(email);

            // Kullanici enumeration'ini engellemek icin: kullanici bulunamasa bile basarili donuyoruz.
            if (user is null)
                return Result.Success();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            // Token URL query'sinde tasinacagi icin encode ediyoruz (+ / = karakterleri bozulmasin).
            var resetLink =
                $"{_mailSettings.Value.ClientUrl}/reset-password" +
                $"?token={encodedToken}" +
                $"&email={Uri.EscapeDataString(email)}" +
                $"&token={Uri.EscapeDataString(token)}";

            const string subject = "Şifre Sıfırlama Talebi";
            var body = $@"
        <p>Merhaba,</p>
        <p>Hesabınız için şifre sıfırlama talebinde bulunuldu. Aşağıdaki bağlantıya tıklayarak yeni şifrenizi belirleyebilirsiniz:</p>
        <p><a href=""{resetLink}"">Şifremi Sıfırla</a></p>
        <p>Bu talebi siz yapmadıysanız bu e-postayı yok sayabilirsiniz.</p>";

            await _emailService.SendEmailAsync(email, subject, body, cancellationToken);

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken)
        { 
             
            var info = await _userRepository.GetByEmailForRememberPasswordAsync(email, cancellationToken); // sadece Guid? döner
            if (info is null)
                return Result.Failure(new Error("RESET_PASSWORD_INVALID", "Şifre sıfırlama işlemi geçersiz."));

            if (info.TenantId is Guid tid)
                _tenantService.SetTenantId(tid);
             
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure(new Error("RESET_PASSWORD_INVALID", "Şifre sıfırlama işlemi geçersiz."));

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            return Result.Success();
        }
    }
}
