using System.ComponentModel.DataAnnotations;

namespace Challenge_Sprint_3.Models
{
    public class UserLoginRequest
    {
        [Required, EmailAddress(ErrorMessage = "O email deve ser um email valido.")]
        public string email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
