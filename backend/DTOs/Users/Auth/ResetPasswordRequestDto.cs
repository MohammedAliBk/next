using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.DTOs.Users.Auth
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string Token { get; set; } = null!;
        
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = null!;
        
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}

