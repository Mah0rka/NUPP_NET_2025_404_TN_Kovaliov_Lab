using System.ComponentModel.DataAnnotations;

namespace Fish.REST.Models
{
    // DTO для реєстрації нового користувача
    public class RegisterDto
    {
        // Email користувача
        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; } = string.Empty;

        // Пароль
        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [MinLength(6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
        public string Password { get; set; } = string.Empty;

        // Підтвердження пароля
        [Required(ErrorMessage = "Підтвердження пароля є обов'язковим")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Повне ім'я користувача
        public string? FullName { get; set; }
    }
}


