namespace TodoListAPI.DTOs.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? PictureUrl { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

