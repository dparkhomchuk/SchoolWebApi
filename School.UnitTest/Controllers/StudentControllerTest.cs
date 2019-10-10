using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using School.Controllers;
using School.Interface;
using School.Models;
using School.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.UnitTest.Controllers
{
    [TestFixture]
    public class StudentControllerTest
    {
        IStudentService studentService;
        StudentController studentController;

        List<StudentDTO> studentDTOs;

        [SetUp]
        public void Initialization()
        {
            studentDTOs = new List<StudentDTO>()
            {
                new StudentDTO(){ Id = 1, Name = "Victor", Surname = "Dyshkant", Age = 19 },
                new StudentDTO(){ Id = 2, Name = "Alla", Surname = "Dyshkant", Age = 17 },
                new StudentDTO(){ Id = 3, Name = "Oleg", Surname = "Dyshkant", Age = 45 }
            };

            var mock = new Mock<IStudentService>();
            mock.Setup(x => x.Get(It.IsAny<int>())).Returns((int Id) => Task.FromResult(studentDTOs.FirstOrDefault(x => x.Id == Id)));
            mock.Setup(x => x.GetAll()).Returns(() => Task.FromResult((IEnumerable<StudentDTO>)studentDTOs));

            studentService = mock.Object;
            var mapper = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new Mapping() });
            }).CreateMapper();

            studentController = new StudentController(studentService, mapper);
        }

        [Test]
        public async Task GetById()
        {
            //Arrenge
            int studentId = 2;
            //Act
            ActionResult actionResult = await studentController.Get(studentId);
            var okResult = actionResult as ObjectResult;
            StudentModel student = (StudentModel)okResult.Value;

            studentDTOs.FirstOrDefault(x => x.Id == studentId).Should().BeEquivalentTo(student);
        }

        [Test]
        public async Task GetAll()
        {
            //Arrenge

            //Act
            ActionResult actionResult = await studentController.GetAll();
            var okResult = actionResult as ObjectResult;
            IEnumerable<StudentModel> studentModels = (IEnumerable<StudentModel>)okResult.Value;

            //Assert
            int i = 0;
            foreach (StudentModel studentModel in studentModels)
            {
                studentModel.Should().BeEquivalentTo(studentDTOs[i]);
                i++;
            }
        }
    }
}
