namespace TodoListAPI.Repositories.SectionsRepos
{
    using Microsoft.EntityFrameworkCore;
    using TodoListAPI.Models.Domain;
    using TodoListAPI.Models.Infrastructure;

    public class SectionRepository : ISectionRepository
    {
        private readonly TodoListDbContext _dbContext;

        public SectionRepository(TodoListDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task UpdateSectionAsync(Section section)
        {
            _dbContext.Sections.Update(section);
            await SaveAsync();
        }

        public async Task DeleteSectionAsync(Section section)
        {
            _dbContext.Sections.Remove(section);
            await SaveAsync();
        }

        public async Task CreateSectionAsync(Section section)
        {
            await _dbContext.Sections.AddAsync(section);
            await SaveAsync();
        }

        public async Task<Section?> GetSectionByIdAsync(Guid sectionId, Guid userId)
        {
            return await _dbContext.Sections
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sectionId && s.UserId == userId);
        }

        public async Task<IEnumerable<Section>> GetAllSectionsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Sections
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Section>> GetAll()
        {
            return await _dbContext.Sections
                .AsNoTracking()
                .ToListAsync();
        }

        // Assumption: this repository is called only after validation,
        // so the section is guaranteed to exist.


        private Task SaveAsync() => _dbContext.SaveChangesAsync();
    }
}
