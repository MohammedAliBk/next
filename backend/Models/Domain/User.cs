namespace TodoListAPI.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? PictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }

}
