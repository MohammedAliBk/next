namespace TodoListAPI.DTOs.Tasks
{
    public class GetTaskInfo
    {
        public Guid PublicId { get; init; }
        public string Title { get; init; } = null!;
        public string? Note { get; init; }
        public Models.Enums.TaskStatus Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
