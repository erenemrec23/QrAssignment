using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using QrAssignment.Application.Interfaces;
using System.Collections.Generic; // IEnumerable için gerekli
using System.Linq; // LINQ metotları (Select) için gerekli

namespace QrAssignment.Presentation.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        // GetClaim yerine GetClaims yaptık ve dönüş tipini IEnumerable<string> olarak değiştirdik
        public IEnumerable<string> GetClaims(string claimType)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                // Null reference hatalarını önlemek için boş liste dönüyoruz
                return Enumerable.Empty<string>();
            }

            // FindAll metodu ile hem User'dan hem Role'den gelen tüm aynı isimli claimleri yakalıyoruz
            return user.FindAll(claimType).Select(c => c.Value);
        }
    }
}