
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using School.Database.UnitOfWork;
using School.Entity;
using School.Interface;
using School.Service;
using School.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Database.Integration.Tests.Service
{
    [TestFixture]
    public class StudentServiceIntegrationTest
    {
        StudentService studentService;
        List<StudentDTO> studentDTOs;
        IMapper mapper;
        [OneTimeSetUp]
        public void Initialization()
        {
            mapper = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new Mapping());
            }).CreateMapper();

            studentDTOs = new List<StudentDTO>()
            {
                new StudentDTO(){ Id = 1, Name = "Victor", Surname = "Dyshkant", Age = 19 },
                new StudentDTO(){ Id = 2, Name = "Alla", Surname = "Dyshkant", Age = 17 },
                new StudentDTO(){ Id = 3, Name = "Oleg", Surname = "Dyshkant", Age = 45 }
            };

            var options = new DbContextOptionsBuilder<SchoolDataBase>()
                .UseInMemoryDatabase("Server=(localdb)\\mssqllocaldb;Database=StudentServiceIntegrationTest;Trusted_Connection=True;ConnectRetryCount=0")
                .Options;

            SchoolDataBase context = new SchoolDataBase(options);
            context.Students.AddRange(mapper.Map<IEnumerable<Student>>(studentDTOs));
            context.SaveChanges();
            EFUnitOfWork unit = new EFUnitOfWork(context);

            studentService = new StudentService(unit, mapper);
        }


        [Test]
        public async Task GetUserById()
        {
            //Arrenge
            int studentId = 2;

            //Act
            StudentDTO studentDTO = await studentService.Get(studentId);

            //Assert
            studentDTOs.FirstOrDefault(x => x.Id == studentId).Should().BeEquivalentTo(studentDTO);
        }

        [Test]
        public async Task GetAllStudents()
        {
            //Act
            IEnumerable<StudentDTO> studentFoundDTOs = await studentService.GetAll();

            //Assert
            studentDTOs.Should().BeEquivalentTo(studentFoundDTOs);
        }

        [Test]
        public async Task AddStudent()
        {
            //Arrenge
            StudentDTO studentDTO = new StudentDTO() { Id = studentDTOs.Count() + 1, Name = "Leonid", Surname = "Rybitsky", Age = 12 };
            studentDTOs.Add(studentDTO);
            //Act
            await studentService.Create(studentDTO);
            StudentDTO studentFound = await studentService.Get(studentDTO.Id);
            //Assert
            studentDTO.Should().BeEquivalentTo(studentFound);
        }
    }
}
