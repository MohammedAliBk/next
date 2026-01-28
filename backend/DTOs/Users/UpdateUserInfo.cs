namespace TodoListAPI.DTOs.Users
{
    public class UpdateUserInfo
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PictureUrl { get; set; }
    }
}