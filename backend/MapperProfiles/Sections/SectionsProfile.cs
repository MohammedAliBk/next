namespace TodoListAPI.MapperProfiles.Sections
{
    using AutoMapper;
    using TodoListAPI.DTOs.Sections;
    using TodoListAPI.Models.Domain;

    public class SectionsProfile : Profile
    {
        public SectionsProfile()
        {
            CreateMap<SetSectionInfo, Section>();

            CreateMap<UpdateSectionInfo, Section>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Section, GetSectionInfo>();
        }
    }
}
