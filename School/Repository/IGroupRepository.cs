using School.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Repository
{
    public interface IGroupRepository
    {
        void Create(Group group);
        Task<Group> Get(int id);
        Task<IEnumerable<Group>> GetAll();
    }
}
