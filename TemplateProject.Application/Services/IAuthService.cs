using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Services
{
    public interface IAuthService
    {
        // Giriş başarılı olursa geriye Token (string) dönecek
        Task<LoginCommandResponse> LoginAsync(string email, string password, CancellationToken cancellationToken);

        Task CreateAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken);
    }
}
