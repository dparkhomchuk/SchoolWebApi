using System.Collections.Generic;
using System.Threading.Tasks;


namespace School.Interface
{
    public interface IStudentService
    {
        Task Create(StudentDTO studentDTO);
        Task<StudentDTO> Get(int id);
        Task<IEnumerable<StudentDTO>> GetAll();

        Task Update(StudentDTO studentDTO);
        //Task Delete(int id);

    }
}
