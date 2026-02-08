namespace TodoListAPI.Models.Domain
{
    public class TaskItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Note { get; set; }

        public TaskStatus Status { get; set; }

        public Guid SectionId { get; set; }
        public Section Section { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
