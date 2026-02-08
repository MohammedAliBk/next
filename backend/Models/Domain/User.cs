using TodoListAPI.Models.Enums;

namespace TodoListAPI.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? PictureUrl { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
