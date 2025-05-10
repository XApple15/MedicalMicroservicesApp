using Microsoft.AspNetCore.Identity;

namespace AuthService.Models.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);

    }
}
