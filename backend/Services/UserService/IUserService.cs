using TodoListAPI.DTOs.Users.Auth;
using TodoListAPI.DTOs.Users;

namespace TodoListAPI.Services.UserService
{
    public interface IUserService
    {
        Task<UserResponseDto> UpdateUserPasswordAsync(Guid id, ChangePasswordDto dto);
        Task DisableUserAsync(Guid id);

        Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto , string ip, string userAgent);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, string ip, string ua);
        Task<AuthResponseDto> RefreshAsync(string refreshToken, string ip, string ua);
        Task RevokeAsync(string refreshToken, string? reason = null);
        Task SignOutAsync(Guid userId, string? reason = null);

        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordRequestDto dto);
    }
}
