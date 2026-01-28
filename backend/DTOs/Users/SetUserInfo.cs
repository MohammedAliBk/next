namespace TodoListAPI.DTOs.Users
{
    public class SetUserInfo
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PictureUrl { get; set; }
    }
}