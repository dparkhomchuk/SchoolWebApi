
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using School.Entity;
using School.Interface;
using School.Repository;
using School.Service;
using School.UnitOfWork;
using School.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.UnitTest
{
    [TestFixture]
    public class StudentServiceTest
    {
        StudentService studentService;
        List<Student> students;

        [SetUp]
        public void Initialize()
        {
            students = new List<Student>(){
            new Student () { Id = 0, Name = "Victor", Surname = "Dyshkant", Age = 19 },
            new Student() { Id = 1, Name = "Alla", Surname = "Dyshkant", Age = 17 },
            new Student() { Id = 2, Name = "Oleg", Surname = "Dyshkant", Age = 45 }};

            var mockStudentRepository = new Mock<IStudentRepository>();
            mockStudentRepository.Setup(x => x.Get(It.IsAny<int>())).Returns((int id) => Task.FromResult(students.FirstOrDefault(x => x.Id == id)));
            mockStudentRepository.Setup(x => x.GetAll()).Returns(Task.FromResult((IEnumerable<Student>)students));

            var mockUnitOfWork = new MockRepository(MockBehavior.Default);
            IUnitOfWork unit = mockUnitOfWork.Of<IUnitOfWork>()
                 .Where(x => x.StudentRepository == mockStudentRepository.Object)
                 .First();


            var mapper = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new Mapping() });
            }).CreateMapper();

            studentService = new StudentService(unit, mapper);
        }

        [Test]
        public async Task GetStudent_ById()
        {
            //Arrenge
            int id = 1;
            //Act
            StudentDTO studentDTO = await studentService.Get(id);

            //Assert
            students.FirstOrDefault(x => x.Id == id).Should().BeEquivalentTo(studentDTO);
        }

        [Test]
        public async Task GetAllStudents()
        {

            //Act
            IEnumerable<StudentDTO> studentDTOs = await studentService.GetAll();

            //Assert
            students.Should().BeEquivalentTo(studentDTOs);
        }

        //[Test]
        //public async Task AddStudent()
        //{
        //    //Arrenge
        //    StudentService studentService = new StudentService(unit, mapper);
        //    StudentDTO studentDTO = new StudentDTO() { Name = "Olga", Surname = "Pupkina", Age = 15 };
        //    //Act
        //    await studentService.Create(studentDTO);
        //    //Assert

        //}
    }
}
