using System.ComponentModel.DataAnnotations;

namespace Fish.REST.Models
{
    // DTO для входу користувача
    public class LoginDto
    {
        // Email користувача
        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; } = string.Empty;

        // Пароль
        [Required(ErrorMessage = "Пароль є обов'язковим")]
        public string Password { get; set; } = string.Empty;
    }
}

