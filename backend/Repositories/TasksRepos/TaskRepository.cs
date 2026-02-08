
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models.Domain;
using TodoListAPI.Models.Infrastructure;

namespace TodoListAPI.Repositories.TasksRepos
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TodoListDbContext _dbContext;

        public TaskRepository(TodoListDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(TaskItem task)
        {
            await _dbContext.Tasks.AddAsync(task);
            await SaveAsync();
        }

        public async Task Delete(TaskItem task)
        {
            _dbContext.Tasks.Remove(task);
            await SaveAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAll()
        {
            return await _dbContext.Tasks
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksBySectionAndUserAsync(Guid sectionId, Guid userId)
        {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Include(t => t.Section)
                .Where(t => t.SectionId == sectionId && t.Section.UserId == userId)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskBySectionAndUserAsync(Guid id , Guid sectionId, Guid userId)
        {
            return await _dbContext.Tasks.AsNoTracking()
                .Include(t => t.Section)
                .FirstOrDefaultAsync(t => t.Id == id && t.SectionId == sectionId && t.Section.UserId == userId);
        }

        public async Task Update(TaskItem task)
        {
            _dbContext.Tasks.Update(task);
            await SaveAsync();
        }

        private Task SaveAsync() => _dbContext.SaveChangesAsync();
    }
}
