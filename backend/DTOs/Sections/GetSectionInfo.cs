namespace TodoListAPI.DTOs.Sections
{
    public class GetSectionInfo
    {
        public Guid PublicId { get; init; }
        public string SectionName { get; init; } = null!;
        public string? Note { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
