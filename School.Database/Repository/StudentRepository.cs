using Microsoft.EntityFrameworkCore;
using School.Entity;
using School.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Database.Repository
{
    public class StudentRepository : IStudentRepository
    {
        SchoolDataBase _context;
        public StudentRepository(SchoolDataBase context)
        {
            _context = context;
        }
        public void Create(Student student)
        {
            _context.Students.Add(student);
        }

        //public void Delete(Student student)
        //{
        //    _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        //}
        public void Update(Student student)
        {
            _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task<Student> Get(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

    }
}
