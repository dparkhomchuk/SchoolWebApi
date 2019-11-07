using AutoMapper;
using School.Entity;
using School.Event;
using School.Interface;
using System.Linq;
using School.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Service
{
    public class GroupService : IGroupService
    {
        IUnitOfWork _unit;
        IMapper _mapper;
        readonly IEventPublisher _eventPublisher;
        public GroupService(IUnitOfWork unit, IMapper mapper, IEventPublisher eventPublisher)
        {
            _unit = unit;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }
        public async Task Create(GroupDTO groupDTO)
        {
            Group group = _mapper.Map<Group>(groupDTO);
            _unit.GroupRepository.Create(group);
            await _unit.Save();
        }

        public async Task<GroupDTO> Get(int groupId)
        {
            Group group = await _unit.GroupRepository.Get(groupId);
            return _mapper.Map<GroupDTO>(group);
        }
        public async Task<GroupDTOWithFreePlaces> GetWithFreePlace(int groupId)
        {
            Group group = await _unit.GroupRepository.Get(groupId);
            return _mapper.Map<GroupDTOWithFreePlaces>(group);
        }
        public async Task AssignStudentToGroup(int studentId, int groupId)
        {
            Group group = await _unit.GroupRepository.Get(groupId);
            Student student = await _unit.StudentRepository.Get(studentId);
            group.AddStudentToGroup(student);
            await _unit.Save();
            group.DomainEvents.ForEach(domainEvent => _eventPublisher.Publish(domainEvent));
        }
        public async Task<IEnumerable<GroupDTO>> GetAll()
        {
            IEnumerable<Group> groups = await _unit.GroupRepository.GetAll();
            return _mapper.Map<IEnumerable<GroupDTO>>(groups);
        }
        public async Task KickStudentFromGroup(int studentId, int groupId)
        {
            Group group = await _unit.GroupRepository.Get(groupId);
            Student student = await _unit.StudentRepository.Get(studentId);

            group.DeleteStudentFromGroup(student);

            await _unit.Save();
        }
        public async Task<IEnumerable<StudentDTO>> GetStudentsByGroupId(int groupId)
        {
            Group group = await _unit.GroupRepository.Get(groupId);
            return _mapper.Map<IEnumerable<StudentDTO>>(group.Students);
        }

        
    }
}
