namespace TodoListAPI.Models.Domain
{
    public class Section
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }

        public string SectionName { get; set; } = null!;
        public string? Note { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

}
