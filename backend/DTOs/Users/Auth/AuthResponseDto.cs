namespace TodoListAPI.DTOs.Users.Auth
{
    public class AuthResponseDto
    {
        public UserResponseDto User { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public int AccessTokenExpiresInSeconds { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
        public DateTime? AccessTokenExpiresAt { get; set; }
    }
}

