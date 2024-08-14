using System.ComponentModel.DataAnnotations;

namespace API.Dto.Account
{
    public class LoginDto
    {
        [Required]
        public string? UsernameOrEmail { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
