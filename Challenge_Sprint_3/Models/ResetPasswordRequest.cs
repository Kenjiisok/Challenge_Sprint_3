using System.ComponentModel.DataAnnotations;

namespace Challenge_Sprint_3.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(7, ErrorMessage = "Por favor insira uma senha com 7 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
