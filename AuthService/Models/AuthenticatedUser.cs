using System.Data;

namespace AuthService.Models
{
    public class AuthenticatedUser
    {
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
