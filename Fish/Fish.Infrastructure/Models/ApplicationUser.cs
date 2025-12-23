using Microsoft.AspNetCore.Identity;

namespace Fish.Infrastructure.Models
{
    // Модель користувача для Identity
    public class ApplicationUser : IdentityUser
    {
        // Повне ім'я користувача
        public string? FullName { get; set; }

        // Дата створення облікового запису
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

