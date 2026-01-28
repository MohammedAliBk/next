namespace TodoListAPI.DTOs.Tasks
{
    public class UpdateTaskInfo
    {
        public string? Title { get; set; }
        public string? Note { get; set; }
        public Models.Enums.TaskStatus? Status { get; set; }
        public int? SectionId { get; set; }
    }
}
