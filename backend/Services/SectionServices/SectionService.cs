using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TodoListAPI.DTOs.Sections;
using TodoListAPI.Models.Domain;
using TodoListAPI.Repositories.SectionsRepos;
using TodoListAPI.SignalR;

namespace TodoListAPI.Services.SectionServices
{
    public class SectionService : ISectionServicecs
    {
        private ISectionRepository? _sectionRepo;
        private IMapper _mapper;
        private readonly IHubContext<SectionsHub> _hub;
        public SectionService(ISectionRepository sectionRepository, IMapper mapper, IHubContext<SectionsHub> hub)
        {
            _sectionRepo = sectionRepository;
            _hub = hub;
            _mapper = mapper;
        }


        public async Task<GetSectionInfo> CreateSectionAsync(SetSectionInfo section, Guid UserId)
        {
            var sec = _mapper.Map<Section>(section);
            sec.UserId = UserId;

            await _sectionRepo!.CreateSectionAsync(sec);

            var entity = await _sectionRepo.GetSectionByIdAsync(sec.Id, UserId);

            var result = _mapper.Map<GetSectionInfo>(entity);

             await _hub.Clients
            .User(UserId.ToString())
            .SendAsync("SectionCreated", result);

            return result;
        }

        public async Task<bool> DeleteSectionAsync(Guid publicId, Guid UserId)
        {
            var entity = await _sectionRepo!.GetSectionByIdAsync(publicId, UserId);

            if (entity is null)
                return false;

            await _sectionRepo.DeleteSectionAsync(entity);

            await _hub.Clients
                .User(UserId.ToString())
                .SendAsync("SectionDeleted", publicId);

            return true;
        }

        public async Task<IEnumerable<GetSectionInfo>> GetAll()
        {
            var entities = await _sectionRepo!.GetAll();
            return _mapper.Map<IEnumerable<GetSectionInfo>>(entities);
        }

        public async Task<IEnumerable<GetSectionInfo>> GetAllSectionsByUserIdAsync(Guid userId)
        {
            var entities = await _sectionRepo!.GetAllSectionsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<GetSectionInfo>>(entities);
        }

        public async Task<GetSectionInfo?> GetSectionByIdAsync(Guid sectionId, Guid userId)
        {
            var entity = await _sectionRepo!.GetSectionByIdAsync(sectionId, userId);

            if (entity is null)
                return null;

            return _mapper.Map<GetSectionInfo>(entity);
        }

        public async Task<GetSectionInfo> UpdateSectionAsync(UpdateSectionInfo section, Guid UserId, Guid SectionId)
        {
            var entity = await _sectionRepo!.GetSectionByIdAsync(SectionId, UserId);

            if (entity is null)
                throw new KeyNotFoundException($"Section with ID {SectionId} not found for user {UserId}");

            _mapper.Map(section, entity);

            await _sectionRepo.UpdateSectionAsync(entity);

            var result = _mapper.Map<GetSectionInfo>(entity);

            await _hub.Clients
                .User(UserId.ToString())
                .SendAsync("SectionUpdated", result);

            return result;
        }
    }
}
