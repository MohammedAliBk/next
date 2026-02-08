using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoListAPI.Cloudinary;
using TodoListAPI.DTOs.Users;
using TodoListAPI.DTOs.Users.Auth;
using TodoListAPI.Helper;
using TodoListAPI.JwtService;
using TodoListAPI.Models.Domain;
using TodoListAPI.Models.Enums;
using TodoListAPI.Models.Infrastructure;
using TodoListAPI.Repositories.UserRepo;
using TodoListAPI.Security;

namespace TodoListAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwt;
        private readonly IPasswordHasher _hasher;
        private readonly IConfiguration _cfg;
        private readonly TodoListDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly ICloudinaryService _cloudinary;

        public UserService(IUserRepository repo, IMapper mapper, IJwtTokenService jwt, IPasswordHasher hasher, IConfiguration cfg, TodoListDbContext dbContext, IEmailSender emailSender, ICloudinaryService cloudinaryService)
        {
            _repo = repo;
            _mapper = mapper;
            _jwt = jwt;
            _hasher = hasher;
            _cfg = cfg;
            _dbContext = dbContext;
            _emailSender = emailSender;
            _cloudinary = cloudinaryService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, string ip, string ua)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null || !_hasher.Verify(user.PasswordHash, dto.Password))
                throw new SecurityTokenException("Invalid credentials.");

            if (!user.IsActive)
                throw new SecurityTokenException("User account is disabled.");

            var accessToken = _jwt.GenerateAccessToken(user);
            var rememberDays = int.Parse(_cfg["Jwt:RememberMeRefreshTokenExpirationDays"] ?? "30");
            var normalDays = int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var validityDays = dto.RememberMe ? rememberDays : normalDays;

            var refreshToken = _jwt.CreateRefreshToken(user.Id, TimeSpan.FromDays(validityDays), dto.Device, ip, ua);

            await _repo.AddRefreshTokenAsync(refreshToken);
            await _repo.SaveChangesAsync();

            return new AuthResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresInSeconds = int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"] ?? "15") * 60,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"] ?? "15"))
            };
        }

        public async Task<AuthResponseDto> RefreshAsync(string token, string ip, string ua)
        {
            var existing = await _repo.GetRefreshTokenAsync(token);
            if (existing == null)
                throw new SecurityTokenException("Invalid refresh token.");
            if (existing.IsRevoked)
                throw new SecurityTokenException("Refresh token revoked.");
            if (existing.ExpiresAt <= DateTime.UtcNow)
                throw new SecurityTokenException("Refresh token expired.");

            var user = existing.User ?? throw new SecurityTokenException("User not found for refresh token.");

            var normalDays = int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var newRefresh = _jwt.CreateRefreshToken(user.Id, TimeSpan.FromDays(normalDays), existing.Device, ip, ua);

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            existing.RevokeReason = "Rotated on refresh";
            existing.ReplacedByToken = newRefresh.Token;

            await _repo.AddRefreshTokenAsync(newRefresh);
            await _repo.UpdateRefreshTokenAsync(existing);
            await _repo.SaveChangesAsync();

            var newAccessToken = _jwt.GenerateAccessToken(user);

            return new AuthResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                AccessToken = newAccessToken,
                RefreshToken = newRefresh.Token,
                AccessTokenExpiresInSeconds = int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"] ?? "15") * 60,
                RefreshTokenExpiresAt = newRefresh.ExpiresAt,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"] ?? "15"))
            };
        }

        public async Task RevokeAsync(string refreshToken, string? reason = null)
        {
            var existing = await _repo.GetRefreshTokenAsync(refreshToken);
            if (existing == null)
                return;

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            existing.RevokeReason = reason ?? "Revoked by user";

            await _repo.UpdateRefreshTokenAsync(existing);
            await _repo.SaveChangesAsync();
        }

        public async Task SignOutAsync(Guid userId, string? reason = null)
        {
            await _repo.RevokeAllForUserAsync(userId, reason ?? "User signed out");
            await _repo.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto , string ip, string userAgent)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new SecurityTokenException("Passwords do not match.");

            var email = EmailNormalizer.NormalizeEmail(dto.Email);
            if (await _dbContext.Users.AnyAsync(u => u.Email == email))
                throw new SecurityTokenException("Email already exists.");

            var roleParsed = dto.Role;
            var picture = await _cloudinary.UploadAsync(dto.PictureUrl);

            var user = _mapper.Map<User>(dto);
            user.PictureUrl = picture.ToString();
            user.PasswordHash = _hasher.Hash(dto.Password);
            user.Role = roleParsed;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsActive = true;

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            var normalDays = int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.CreateRefreshToken(user.Id, TimeSpan.FromDays(normalDays), device: null, ip: ip, ua: userAgent);

            await _repo.AddRefreshTokenAsync(refreshToken);
            await _repo.SaveChangesAsync();

            return new AuthResponseDto
            {
                User = _mapper.Map<UserResponseDto>(user),
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresInSeconds = int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"] ?? "15") * 60,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"] ?? "15"))
            };
        }

        public async Task<UserResponseDto> UpdateUserPasswordAsync(Guid id, ChangePasswordDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found.");

            if (!user.IsActive) throw new BadHttpRequestException("User is disabled.");

            if (!_hasher.Verify(user.PasswordHash, dto.CurrentPassword))
                throw new SecurityTokenException("Current password is incorrect.");

            if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 8)
                throw new SecurityTokenException("New password must be at least 8 characters.");

            if (dto.CurrentPassword == dto.NewPassword)
                throw new ArgumentException("New password must be different from current password.");

            user.PasswordHash = _hasher.Hash(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.RevokeAllForUserAsync(user.Id, "Password changed");
            await _repo.UpdateAsync(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task DisableUserAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found.");

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(user);
            await _repo.RevokeAllForUserAsync(user.Id, "User disabled");
            await _repo.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var normalizedEmail = EmailNormalizer.NormalizeEmail(email);
            var user = await _repo.GetByEmailAsync(normalizedEmail);

            if (user == null || !user.IsActive)
                return;

            var token = Guid.NewGuid().ToString("N");

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                Used = false,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddPasswordResetTokenAsync(resetToken);
            await _repo.SaveChangesAsync();

            var frontendUrl = _cfg["App:FrontendUrl"] ?? "https://localhost:3000";
            var resetLink = $"{frontendUrl}/reset-password?token={token}";

            var emailBody = $@"
            Hello {user.Name},

            You requested to reset your password. Click the link below to reset it:

            {resetLink}

            This link will expire in 15 minutes.

            If you didn't request this, please ignore this email.

            Best regards,
            TodoList Team
            ";

            await _emailSender.SendAsync(user.Email, "Reset Your Password", emailBody);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new SecurityTokenException("Passwords do not match.");

            if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 8)
                throw new SecurityTokenException("Password must be at least 8 characters.");

            var resetToken = await _repo.GetValidPasswordResetTokenAsync(dto.Token);
            if (resetToken == null)
                throw new SecurityTokenException("Invalid or expired reset token.");

            var user = resetToken.User;
            if (user == null)
                throw new SecurityTokenException("User not found for reset token.");

            if (!user.IsActive)
                throw new SecurityTokenException("User account is disabled.");

            user.PasswordHash = _hasher.Hash(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            resetToken.Used = true;

            await _repo.RevokeAllForUserAsync(user.Id, "Password reset");

            await _repo.UpdateAsync(user);
            await _repo.UpdatePasswordResetTokenAsync(resetToken);
            await _repo.SaveChangesAsync();
        }
    }
}
