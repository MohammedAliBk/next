using TodoListAPI.Models.Domain;

namespace TodoListAPI.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();

        Task AddRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetActiveRefreshTokensForUserAsync(Guid userId);
        Task UpdateRefreshTokenAsync(RefreshToken token);
        Task RevokeAllForUserAsync(Guid userId, string reason);
        Task<RefreshToken?> GetRefreshTokenByUserIdAsync(Guid userId);

        Task AddPasswordResetTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetValidPasswordResetTokenAsync(string token);
        Task UpdatePasswordResetTokenAsync(PasswordResetToken token);
    }
}
