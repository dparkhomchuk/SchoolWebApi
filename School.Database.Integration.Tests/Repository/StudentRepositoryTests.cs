using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using School.Database.Repository;
using School.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using School.Interface;

namespace School.Database.Integration.Tests.Repository
{
    [TestFixture]
    public class StudentRepositoryTests
    {
        SchoolDataBase _context;
        List<Student> students;
        private StudentRepository studentRepository;
        [OneTimeSetUp]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<SchoolDataBase>()
            .UseInMemoryDatabase("Server=(localdb)\\mssqllocaldb;Database=StudentRepositoryTests;Trusted_Connection=True;ConnectRetryCount=0")
            .Options;
            _context = new SchoolDataBase(options);

            students = new List<Student>() {new Student(){ Name = "Victor",Surname = "Dyshkant",Age = 19 },
            new Student() { Name = "Alla",Surname = "Dyshkant",Age = 18 },
            new Student() { Name = "Oleg",Surname = "Dyshkant",Age = 45 }};


            _context.Students.AddRange(students);
            _context.SaveChanges();
            studentRepository = new StudentRepository(_context);
        }

        [Test]
        public async Task GetAllStudents_In_Database()
        {
            //Act
            IEnumerable<Student> currentStudents = await studentRepository.GetAll();

            //Assert
            students.Should().BeEquivalentTo(currentStudents);
        }

        [Test]
        public async Task AddStudent_To_Database()
        {
            //Arrenge
            Student student = new Student() { Name = "Vlad", Surname = "Brechlo", Age = 18 };
            students.Add(student);

            //Act
            studentRepository.Create(student);
            await _context.SaveChangesAsync();

            //Assert
            _context.Students.FirstOrDefault(x => x.Id == student.Id).Should().BeEquivalentTo(student);
        }

        [Test]
        public async Task GetStudent_ById()
        {
            //Arrenge
            Student student = new Student() { Name = "Vlad", Surname = "Brechlo", Age = 18 };
            students.Add(student);

            //Act
            studentRepository.Create(student);
            await _context.SaveChangesAsync();

            Student foundStudent = await studentRepository.Get(student.Id);

            //Assert
            student.Should().BeEquivalentTo(foundStudent);
        }

        [Test]
        public async Task UpdateStudent()
        {
            //Arrenge
            int studentId = 1;
            Student student = students.FirstOrDefault(x => x.Id == studentId);
            student.Age = 15;
            student.Name = "Viva";
            
            //Act
            studentRepository.Update(student);
            await _context.SaveChangesAsync();

            Student foundStudent = await studentRepository.Get(studentId);
            //Assert
            student.Should().BeEquivalentTo(foundStudent);
        }

    }
}
