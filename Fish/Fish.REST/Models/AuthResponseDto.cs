namespace Fish.REST.Models
{
    // DTO для відповіді після аутентифікації
    public class AuthResponseDto
    {
        // JWT токен
        public string Token { get; set; } = string.Empty;

        // Email користувача
        public string Email { get; set; } = string.Empty;

        // Ролі користувача
        public IList<string> Roles { get; set; } = new List<string>();
    }
}

