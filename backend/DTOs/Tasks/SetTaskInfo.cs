namespace TodoListAPI.DTOs.Tasks
{
    public class SetTaskInfo
    {
        public string Title { get; set; } = null!;
        public string? Note { get; set; }
        public Models.Enums.TaskStatus Status { get; set; }
        public int SectionId { get; set; }
    }
}
