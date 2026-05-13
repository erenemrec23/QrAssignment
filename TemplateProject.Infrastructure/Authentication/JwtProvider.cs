using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TemplateProject.Application.Features.AuthFeatures.Commands.Login;
using TemplateProject.Application.Interfaces;
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Infrastructure.Authentication
{
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserRefreshTokenRepository _appUserRefreshTokenRepository;
        public JwtProvider(IOptions<JwtOptions> jwtOptions, UserManager<AppUser> userManager, IAppUserRefreshTokenRepository appUserRefreshTokenRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _appUserRefreshTokenRepository = appUserRefreshTokenRepository;
        }

        public async Task<LoginCommandResponse> CreateTokenAsync(AppUser user)
        {
            var claims = new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim("FullName",user.FullName)
            };

            DateTime expires = DateTime.Now.AddHours(1);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            DateTime refreshTokenExpires = expires.AddMinutes(15);
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            if (user.RefreshToken is null)
            {
                // Kullanıcının daha önceden bir token tablosu yoksa (ilk girişiyse) Insert yapılır.
                user.RefreshToken = new AppUserRefreshToken
                {
                    AppUserId = user.Id, // Foreign Key
                    RefreshToken = refreshToken,
                    RefreshTokenExpires = refreshTokenExpires
                };
            }
            else
            {
                // Kullanıcının zaten tablosu varsa, sadece değerleri güncellenir (Update).
                user.RefreshToken.RefreshToken = refreshToken;
                user.RefreshToken.RefreshTokenExpires = refreshTokenExpires;
            }
            await _appUserRefreshTokenRepository.AddAsync(user.RefreshToken);

            LoginCommandResponse response = new(
                token,
                refreshToken, 
                refreshTokenExpires,
                user.Id.ToString()

                );

            return response;
        }
    }
}
