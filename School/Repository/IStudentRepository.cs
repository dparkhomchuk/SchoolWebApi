using School.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Repository
{
    public interface IStudentRepository
    {
        void Create(Student student);
        Task<Student> Get(int studentId);

        void Update(Student student);
        Task<IEnumerable<Student>> GetAll();


    }
}
