
using TemplateProject.Application.Features.AuthFeatures.Commands.Login;
using TemplateProject.Domain.Entity.App;

namespace TemplateProject.Application.Interfaces
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateTokenAsync(AppUser user); // User Domain'den gelecek
    }
}
