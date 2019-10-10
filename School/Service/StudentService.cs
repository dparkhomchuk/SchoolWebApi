using AutoMapper;
using School.Entity;
using School.Interface;
using School.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Service
{
    public class StudentService : IStudentService
    {
        IUnitOfWork _unit;
        readonly IMapper _mapper;
        public StudentService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task Create(StudentDTO studentDTO)
        {
            Student student = _mapper.Map<Student>(studentDTO);
            _unit.StudentRepository.Create(student);
            await _unit.Save();
        }

        //public async Task Delete(int id)
        //{
        //    Student student = await _unit.StudentRepository.Get(id);
        //    _unit.StudentRepository.Delete(student);
        //    await _unit.Save();
        //}
        public async Task Update(StudentDTO studentDTO)
        {
            Student student = await _unit.StudentRepository.Get(studentDTO.Id);

            student.Name = studentDTO.Name;
            student.Surname = studentDTO.Surname;
            student.Age = studentDTO.Age;

            _unit.StudentRepository.Update(student);
            await _unit.Save();
        }
        public async Task<StudentDTO> Get(int id)
        {
            Student student = await _unit.StudentRepository.Get(id);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<IEnumerable<StudentDTO>> GetAll()
        {
            IEnumerable<Student> students = await _unit.StudentRepository.GetAll();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }
    }
}
