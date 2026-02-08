using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.DTOs.Users.Auth
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}

