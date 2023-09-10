using System.ComponentModel.DataAnnotations;

namespace Challenge_Sprint_3.Models
{
    public class UserRegisterRequest
    {
        [Required, EmailAddress(ErrorMessage ="O email deve ser um email valido.")]
        public string email { get; set; } = string.Empty;

        [Required, MinLength(3, ErrorMessage ="Pelo menos 3 caracteres.")]
        public string Name { get; set; }

        [Required, MinLength(7, ErrorMessage ="Por favor insira uma senha com 7 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage ="As senhas não coincidem.")]
        public string ConfirmPassword {  get; set; } = string.Empty;
    }
}
