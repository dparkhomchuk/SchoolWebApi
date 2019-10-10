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

namespace School.UnitTests.Service
{
    [TestFixture]
    public class GroupServiceTests
    {
        IUnitOfWork unit;
        IMapper mapper;

        List<Group> groups;
        List<Student> students;

        [SetUp]
        public void Initialize()
        {
            groups = new List<Group>() {
                new Group(){Id = 1,  Name = "SE-121", Capacity = 20 },
                new Group(){Id = 2,  Name = "SE-221", Capacity = 25 },
                new Group(){Id = 3,  Name = "SE-321", Capacity = 30 },
                new Group(){Id = 4,  Name = "SE-421", Capacity = 18 }};
            students = new List<Student>(){
            new Student(){ Id = 1, Name = "Victor", Surname = "Dyshkant", Age = 19 },
            new Student(){ Id = 2, Name = "Alla", Surname = "Dyshkant", Age = 17 },
            new Student(){ Id = 3, Name = "Oleg", Surname = "Dyshkant", Age = 45 }};


            var mockGroupRepository = new Mock<IGroupRepository>();
            mockGroupRepository.Setup(x => x.Get(It.IsAny<int>())).Returns((int id) => Task.FromResult(groups.FirstOrDefault(g => g.Id == id)));
            mockGroupRepository.Setup(x => x.GetAll()).Returns(Task.FromResult((IEnumerable<Group>)groups));

            var mockStudentRepository = new Mock<IStudentRepository>();
            mockStudentRepository.Setup(x => x.Get(It.IsAny<int>())).Returns((int id) => Task.FromResult(students.FirstOrDefault(s => s.Id == id)));
            mockStudentRepository.Setup(x => x.GetAll()).Returns(Task.FromResult((IEnumerable<Student>)students));

            var mockUnitOfWork = new MockRepository(MockBehavior.Default);
            unit = mockUnitOfWork.Of<IUnitOfWork>()
                .Where(x => x.GroupRepository == mockGroupRepository.Object)
                .Where(x => x.StudentRepository == mockStudentRepository.Object)
                .First();


            mapper = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new Mapping() });
            }).CreateMapper();
        }

        [Test]
        public async Task GetGroup_ById()
        {
            //Arrenge
            GroupService groupService = new GroupService(unit, mapper);
            int id = 2;

            //Act
            GroupDTO groupDTO = await groupService.Get(id);

            //Assert
            groups.FirstOrDefault(x => x.Id == id).Should().BeEquivalentTo(groupDTO);
        }

        [Test]
        public async Task GetAllGroups()
        {
            //Arrenge
            GroupService groupService = new GroupService(unit, mapper);

            //Act
            IEnumerable<GroupDTO> groupsDTO = await groupService.GetAll();

            //Assert
            groups.Should().BeEquivalentTo(groupsDTO);
        }

        [Test]
        public async Task AssignStudentToGroup()
        {
            //Arrenge
            GroupService groupService = new GroupService(unit, mapper);
            int groupId = 1;
            int studentId = 1;
            //Act
            await groupService.AssignStudentToGroup(studentId, groupId);

            //Assert
            Assert.IsTrue(groups.FirstOrDefault(x => x.Id == groupId).Students.Contains(students.FirstOrDefault(x => x.Id == studentId)));
        }

        [Test]
        public async Task KickStudent_From_Group()
        {
            //Arrenge
            GroupService groupService = new GroupService(unit, mapper);
            int groupId = 1;
            int studentId = 1;
            await groupService.AssignStudentToGroup(studentId, groupId);

            //Act
            await groupService.KickStudentFromGroup(studentId, groupId);

            //Assert
            Assert.IsFalse(groups.FirstOrDefault(x => x.Id == groupId).Students.Contains(students.FirstOrDefault(x => x.Id == studentId)));
        }

        [Test]
        public async Task GetStudentsByGroup()
        {
            //Arrenge
            GroupService groupService = new GroupService(unit, mapper);
            int groupId = 1;
            await groupService.AssignStudentToGroup(1, groupId);
            await groupService.AssignStudentToGroup(2, groupId);

            //Act 
            IEnumerable<StudentDTO> studentfoundDTO = await groupService.GetStudentsByGroupId(groupId);

            //Assert
            List<Student> list = new List<Student>();
            list.Add(students.FirstOrDefault(x => x.Id == 1));
            list.Add(students.FirstOrDefault(x => x.Id == 2));


            list.Should().BeEquivalentTo(studentfoundDTO);
             
        }

        [Test]
        public async Task GetGroupWithFreePlace()
        {
            //Arrenge
            int studentId1 = 1;
            int studentId2 = 2;
            int groupId = 1;
            GroupService groupService = new GroupService(unit, mapper);
            await groupService.AssignStudentToGroup(studentId1, groupId);
            await groupService.AssignStudentToGroup(studentId2, groupId);

            //Act
            GroupDTOWithFreePlaces groupDtoWithFreePlaces = await groupService.GetWithFreePlace(groupId);

            //Assert
            Assert.AreEqual(groupDtoWithFreePlaces.FreePlaces, groups.FirstOrDefault(x => x.Id == groupId).Capacity - 2);
        }
    }
}
