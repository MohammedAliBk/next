namespace TodoListAPI.DTOs.Users.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = false;
        public string? Device { get; set; }
    }
}

