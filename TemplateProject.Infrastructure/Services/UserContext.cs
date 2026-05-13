using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TemplateProject.Application.Interfaces;

namespace TemplateProject.Infrastructure.Services; // veya Presentation

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    { 
        if(_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(Guid.TryParse(userId, out var guid))
            {
                return guid;
            }
        }

        return null;
    }
}