using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoListAPI.DTOs.Users;
using TodoListAPI.DTOs.Users.Auth;
using TodoListAPI.Services.UserService;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ua = Request.Headers.UserAgent.ToString();

            try
            {
                var result = await _userService.RegisterAsync(dto , ip, ua);

                if (UseRefreshTokenCookie())
                {
                    SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
                }

                return Ok(result);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ua = Request.Headers.UserAgent.ToString();

            try
            {
                var result = await _userService.LoginAsync(dto, ip, ua);

                if (UseRefreshTokenCookie())
                {
                    SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
                }

                return Ok(result);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ua = Request.Headers.UserAgent.ToString();

            try
            {
                var token = dto.RefreshToken;
                if (string.IsNullOrWhiteSpace(token) && UseRefreshTokenCookie())
                {
                    token = Request.Cookies["refreshToken"];
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest(new { message = "Refresh token missing." });
                }

                var result = await _userService.RefreshAsync(token, ip, ua);

                if (UseRefreshTokenCookie())
                {
                    SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
                }

                return Ok(result);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("revoke")]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Revoke([FromBody] RefreshRequestDto? dto = null)
        {
            // User id is in the "sub" claim (NameIdentifier or Sub), not in Identity.Name
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(sub) || !Guid.TryParse(sub, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            if (dto != null && !string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                await _userService.RevokeAsync(dto.RefreshToken, "Revoked during sign out");
            }
            else if (UseRefreshTokenCookie())
            {
                var cookieToken = Request.Cookies["refreshToken"];
                if (!string.IsNullOrWhiteSpace(cookieToken))
                {
                    await _userService.RevokeAsync(cookieToken, "Revoked during sign out");
                }
            }

            await _userService.SignOutAsync(userId, "User signed out");

            if (UseRefreshTokenCookie())
            {
                Response.Cookies.Delete("refreshToken");
            }

            return Ok(new { message = "Successfully signed out from all devices." });
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            await _userService.ForgotPasswordAsync(dto.Email);
            return Ok(new { message = "If this email is registered, a reset link will be sent." });
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            try
            {
                await _userService.ResetPasswordAsync(dto);
                return Ok(new { message = "Password reset successful." });
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            try
            {
                var result = await _userService.UpdateUserPasswordAsync(userId, dto);
                return Ok(new { message = "Password changed successfully.", user = result });
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private bool UseRefreshTokenCookie()
        {
            return bool.Parse(_configuration["Jwt:UseRefreshTokenCookie"] ?? "false");
        }

        private void SetRefreshTokenCookie(string token, DateTime expiresAt)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiresAt,
                Path = "/"
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}
