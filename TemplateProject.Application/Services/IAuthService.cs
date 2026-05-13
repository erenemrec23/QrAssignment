using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Features.AuthFeatures.Commands.Login;

namespace TemplateProject.Application.Services
{
    public interface IAuthService
    {
        // Giriş başarılı olursa geriye Token (string) dönecek
        Task<LoginCommandResponse> LoginAsync(string email, string password, CancellationToken cancellationToken);

        Task CreateAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken);
    }
}
