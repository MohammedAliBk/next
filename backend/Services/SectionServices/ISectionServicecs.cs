namespace TodoListAPI.Services.SectionServices
{
    using TodoListAPI.DTOs.Sections;
    public interface ISectionServicecs
    {
        Task<GetSectionInfo> CreateSectionAsync(SetSectionInfo section, Guid UserId);
        Task<GetSectionInfo> UpdateSectionAsync(UpdateSectionInfo section, Guid UserId, Guid SectionId);
        Task<bool> DeleteSectionAsync(Guid publicId, Guid UserId);
        Task<IEnumerable<GetSectionInfo>> GetAll();
        Task<GetSectionInfo?> GetSectionByIdAsync(Guid sectionId, Guid userId);
        Task<IEnumerable<GetSectionInfo>> GetAllSectionsByUserIdAsync(Guid userId);
    }
}
