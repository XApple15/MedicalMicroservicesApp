using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
