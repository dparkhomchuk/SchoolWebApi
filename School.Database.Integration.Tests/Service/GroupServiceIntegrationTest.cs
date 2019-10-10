
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using School.Database.UnitOfWork;
using School.Entity;
using School.Exceptions;
using School.Interface;
using School.Service;
using School.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace School.Database.Integration.Tests.Service
{
    [TestFixture]
    public class GroupServiceIntegrationTest
    {
        GroupService groupService;
        SchoolDataBase context;
        List<GroupDTO> groupDTOs;
        List<StudentDTO> studentDTOs;

        IMapper mapper;
        [OneTimeSetUp]
        public void Initialization()
        {
            mapper = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new Mapping() });
            }).CreateMapper();

            groupDTOs = new List<GroupDTO>() {
                new GroupDTO(){Id = 1,  Name = "SE-121", Capacity = 20 },
                new GroupDTO(){Id = 2,  Name = "SE-221", Capacity = 25 },
                new GroupDTO(){Id = 3,  Name = "SE-321", Capacity = 30 },
                new GroupDTO(){Id = 4,  Name = "SE-421", Capacity = 18 }};
            studentDTOs = new List<StudentDTO>()
            {
                new StudentDTO(){ Id = 1, Name = "Victor", Surname = "Dyshkant", Age = 19 },
                new StudentDTO(){ Id = 2, Name = "Alla", Surname = "Dyshkant", Age = 17 },
                new StudentDTO(){ Id = 3, Name = "Oleg", Surname = "Dyshkant", Age = 45 }
            };

            var options = new DbContextOptionsBuilder<SchoolDataBase>()
            .UseInMemoryDatabase("Server=(localdb)\\mssqllocaldb;Database=GroupServiceIntegrationTest;Trusted_Connection=True;ConnectRetryCount=0")
            .Options;

            context = new SchoolDataBase(options);
            context.Groups.AddRange(mapper.Map<IEnumerable<Group>>(groupDTOs));
            context.Students.AddRange(mapper.Map<IEnumerable<Student>>(studentDTOs));
            context.SaveChanges();

            EFUnitOfWork unit = new EFUnitOfWork(context);
            groupService = new GroupService(unit, mapper);

        }
        [Test]
        public async Task GetGroupById()
        {
            //Arrenge
            int groupId = 2;
            //Act 
            GroupDTO groupDTO = await groupService.Get(groupId);
            //Assert
            groupDTO.Should().BeEquivalentTo(groupDTOs.FirstOrDefault(x => x.Id == groupId));
        }
        [Test]
        public async Task GetAllGroups()
        {
            //Act
            IEnumerable<GroupDTO> groupFoundDTOs = await groupService.GetAll();
            //Assert
            groupDTOs.Should().BeEquivalentTo(groupFoundDTOs);
        }
        [Test]
        public async Task AddGroupInDatabase()
        {
            //Arrenge
            GroupDTO groupDTO = new GroupDTO() { Id = groupDTOs.Count() + 1, Name = "SE-621", Capacity = 27 };
            groupDTOs.Add(groupDTO);

            //Act
            await groupService.Create(groupDTO);
            GroupDTO groupFoundDTO = await groupService.Get(groupDTO.Id);

            //Assert
            groupDTO.Should().BeEquivalentTo(groupFoundDTO);
        }

        [Test]
        public async Task AssignStudentToGroup_When_EnoughPlace()
        {
            //Arrenge
            int studentId = 3;
            int groupId = 2;

            //Act
            await groupService.AssignStudentToGroup(studentId, groupId);

            //Assert
            Group group = context.Groups.FirstOrDefault(x => x.Id == groupId);
            Student student = context.Students.FirstOrDefault(x => x.Id == studentId);

            Assert.IsTrue(group.Students.Contains(student));
        }

        [Test]
        [ExpectedException(typeof(InvalidCapacityException))]
        public async Task AssignStudentToGroup_When_NotEnoughPlace()
        {
            //Arrenge
            GroupDTO groupDTO = new GroupDTO() { Id = groupDTOs.Count() + 1, Name = "TestGroup", Capacity = 0 };
            groupDTOs.Add(groupDTO);
            await groupService.Create(groupDTO);
            int studentid = 2;

            //Act
            //Assert
            Assert.ThrowsAsync<InvalidCapacityException>(async () => await groupService.AssignStudentToGroup(studentid, groupDTO.Id));
        }

        [Test]
        public async Task KickOfStudentFromGroup()
        {
            //Arrenge
            int studentId = 1;
            int groupId = 1;
            await groupService.AssignStudentToGroup(studentId, groupId);

            //Act
            await groupService.KickStudentFromGroup(studentId, groupId);

            //Assert
            Group group = context.Groups.FirstOrDefault(x => x.Id == groupId);
            Student student = context.Students.FirstOrDefault(x => x.Id == studentId);

            Assert.IsFalse(group.Students.Contains(student));

        }

    }
}
