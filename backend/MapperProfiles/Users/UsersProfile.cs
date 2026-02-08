namespace TodoListAPI.MapperProfiles.Users
{
    using AutoMapper;
    using TodoListAPI.DTOs.Users;
    using TodoListAPI.Models.Domain;

    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserInfo, User>();
            CreateMap<User, UserResponseDto>();
        }
    }
}