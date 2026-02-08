using TodoListAPI.Models.Enums;

namespace TodoListAPI.DTOs.Users
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public IFormFile? PictureUrl { get; set; }
        public string ConfirmPassword { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}