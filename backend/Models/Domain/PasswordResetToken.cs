namespace TodoListAPI.Models.Domain
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; }
    }
}
