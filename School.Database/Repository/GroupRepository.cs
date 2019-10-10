using Microsoft.EntityFrameworkCore;
using School.Entity;
using School.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Database.Repository
{
    public class GroupRepository : IGroupRepository
    {
        SchoolDataBase _context;

        public GroupRepository(SchoolDataBase context)
        {
            _context = context;
        }
        public void Create(Group group)
        {
            _context.Groups.Add(group);
        }
        //public void Update(Group group)
        //{
        //    _context.Entry(group).State = EntityState.Modified;
        //}
        //public void Delete(Group group)
        //{
        //    _context.Entry(group).State = EntityState.Deleted;
        //}

        public async Task<Group> Get(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Group>> GetAll()
        {
            return await _context.Groups.ToListAsync();
        }


    }
}
