namespace TodoListAPI.Models.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokeReason { get; set; }
        public string? ReplacedByToken { get; set; }

        public string? Device { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
