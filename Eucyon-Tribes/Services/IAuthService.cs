using Eucyon_Tribes.Models;

namespace Eucyon_Tribes.Services
{
    public interface IAuthService
    {
        int CheckJWTCookieValidityReturnsUserId(IRequestCookieCollection cookies);
        string GenerateToken(User user, string purpose);
        int ValidateToken(string token);
    }
}
