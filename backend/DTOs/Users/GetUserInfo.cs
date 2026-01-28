namespace TodoListAPI.DTOs.Users
{
    public class GetUserInfo
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? PictureUrl { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

