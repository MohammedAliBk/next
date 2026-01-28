namespace TodoListAPI.Models.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }

        public string Title { get; set; } = null!;
        public string? Note { get; set; }

        public TaskStatus Status { get; set; }

        public int SectionId { get; set; }
        public Section Section { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
