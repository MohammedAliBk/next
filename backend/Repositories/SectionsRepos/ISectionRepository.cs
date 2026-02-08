namespace TodoListAPI.Repositories.SectionsRepos
{
    using TodoListAPI.Models.Domain;

    public interface ISectionRepository
    {
        Task CreateSectionAsync(Section section);
        Task UpdateSectionAsync(Section section);
        Task DeleteSectionAsync(Section section);
        Task<IEnumerable<Section>> GetAll();
        Task<Section?> GetSectionByIdAsync(Guid sectionId, Guid userId);
        Task<IEnumerable<Section>> GetAllSectionsByUserIdAsync(Guid userId);
    }
}