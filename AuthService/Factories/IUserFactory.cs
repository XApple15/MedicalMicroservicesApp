using AuthService.Models;
using AuthService.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Factories
{
    public interface ILoginResponseFactory
    {
        LoginResponseDTO Create(IdentityUser user, IList<string> roles);
    }
}
