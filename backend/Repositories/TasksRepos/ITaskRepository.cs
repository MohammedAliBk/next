using TodoListAPI.Models.Domain;

namespace TodoListAPI.Repositories.TasksRepos
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAll();
        Task<IEnumerable<TaskItem>> GetTasksBySectionAndUserAsync(Guid sectionId, Guid userId);
        Task<TaskItem?> GetTaskBySectionAndUserAsync(Guid id , Guid sectionId, Guid userId);

        //Task<TaskItem> Create(TaskItem task, Guid sectionId, Guid userId);
        //in service we set sectionId and userId
        Task Create(TaskItem task);

        //Task<TaskItem> Update(TaskItem task, Guid sectionId, Guid userId);
        //in service we set sectionId and userId
        Task Update(TaskItem task);

        //Task<bool> Delete(Guid id, Guid sectionId, Guid userId);
        //in service we set sectionId and userId
        Task Delete(TaskItem task);
    }
}
