
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using School.Interface;
using School.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace School.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class StudentController : ControllerBase
    {
        IStudentService _studentService;
        IMapper _mapper;
        public StudentController(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        [HttpPost("student")]
        public async Task<ActionResult> Add(StudentModel studentModel)
        {
            StudentDTO studentDTO = _mapper.Map<StudentDTO>(studentModel);
            await _studentService.Create(studentDTO);
            return Ok();
        }

        //[HttpDelete("student/{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    await _studentService.Delete(id);
        //    return Ok();
        //}

        [HttpPut("student")]
        public async Task<ActionResult> Update(StudentModel studentModel)
        {
            StudentDTO studentDTO = _mapper.Map<StudentDTO>(studentModel);
            await _studentService.Update(studentDTO);
            return Ok();
        }

        [HttpGet("student/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            StudentDTO studentDTO = await _studentService.Get(id);
            return Ok(_mapper.Map<StudentModel>(studentDTO));
        }

        [HttpGet("students")]
        public async Task<ActionResult> GetAll()
        {
            IEnumerable<StudentDTO> students = await _studentService.GetAll();
            return Ok(_mapper.Map<IEnumerable<StudentModel>>(students));
        }


    }
}