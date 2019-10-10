using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Interface
{
    public interface IGroupService
    {
        Task<GroupDTO> Get(int groupId);

        Task<GroupDTOWithFreePlaces> GetWithFreePlace(int groupId);
        Task<IEnumerable<GroupDTO>> GetAll();
        Task Create(GroupDTO groupDTO);
        Task AssignStudentToGroup(int studentId, int groupId);
        Task KickStudentFromGroup(int studentId, int groupId);

        Task<IEnumerable<StudentDTO>> GetStudentsByGroupId(int groupId);

    }
}
