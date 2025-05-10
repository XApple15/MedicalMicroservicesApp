using AuthService.Models.DTO;
using AuthService.Models.Repository;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Factories
{
    public class LoginResponseFactory : ILoginResponseFactory
    {
        private readonly ITokenRepository _tokenRepository;

        public LoginResponseFactory(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public LoginResponseDTO Create(IdentityUser user, IList<string> roles)
        {
            var token = _tokenRepository.CreateJWTToken(user, roles.ToList());

            return new LoginResponseDTO
            {
                JwtToken = token
            };
        }
    }
}