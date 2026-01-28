namespace TodoListAPI.MapperProfiles.Tasks
{
    using AutoMapper;
    using TodoListAPI.DTOs.Tasks;
    using TodoListAPI.Models.Domain;

    public class TasksProfile : Profile
    {
        public TasksProfile()
        {
            CreateMap<SetTaskInfo, TaskItem>();

            CreateMap<UpdateTaskInfo, TaskItem>()
                .ForAllMembers(o =>
                    o.Condition((src, dest, value) => value != null));

            CreateMap<TaskItem, GetTaskInfo>();
        }
    }
}
