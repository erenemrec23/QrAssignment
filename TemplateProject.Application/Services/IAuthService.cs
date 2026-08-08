using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Services
{
    public interface IAuthService
    { 
        Task<LoginCommandResponse> LoginAsync(string email, string password, CancellationToken cancellationToken);

        Task<Guid> CreateAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken);

        Task<Result> ForgotPasswordAsync(string email, CancellationToken cancellationToken);
        Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken);
    }
}
