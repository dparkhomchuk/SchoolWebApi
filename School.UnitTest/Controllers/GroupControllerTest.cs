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
using School.Web.Models;

namespace School.UnitTest.Controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        IGroupService groupService;
        GroupController groupController;

        List<GroupDTO> groupDTOs;
        private GroupDTOWithFreePlaces groupDtoWithFreePlaces;
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
            groupDTOs = new List<GroupDTO>()
            {
                new GroupDTO(){Id = 1,  Name = "SE-121", Capacity = 20 },
                new GroupDTO(){Id = 2,  Name = "SE-221", Capacity = 25 },
                new GroupDTO(){Id = 3,  Name = "SE-321", Capacity = 30 },
                new GroupDTO(){Id = 4,  Name = "SE-421", Capacity = 18 }
            };
            groupDtoWithFreePlaces = new GroupDTOWithFreePlaces()
            {
                Id = 1,
                Name = "SE-121",
                Capacity = 20,
                FreePlaces = 5
            };

            var mock = new Mock<IGroupService>();
            mock.Setup(x => x.Get(It.IsAny<int>())).Returns((int Id) => Task.FromResult(groupDTOs.FirstOrDefault(x => x.Id == Id)));
            mock.Setup(x => x.GetAll()).Returns(() => Task.FromResult((IEnumerable<GroupDTO>)groupDTOs));
            mock.Setup(x => x.GetWithFreePlace(It.IsAny<int>())).Returns(() => Task.FromResult(groupDtoWithFreePlaces));
            //mock.Setup(x => x.GetStudentsByGroupId(It.IsAny<int>()).Returns(() => Task.FromResult((IEnumerable<GroupDTO>)groupDTOs));
            //mock.Setup(x => x.AssignStudentToGroup(It.IsAny<int>(),It.IsAny<int>())).Callback((int studentId,int groupId)=> 
            //{ 

            //});


            groupService = mock.Object;
            var mapper = new MapperConfiguration(con =>
            {
                con.AddProfiles(new Profile[] { new Mapping() });
            }).CreateMapper();

            groupController = new GroupController(groupService, mapper);
        }

        [Test]
        public async Task GetGroupById()
        {
            //Arrenge
            int groupId = 2;

            //Act
            ActionResult actionResult = await groupController.Get(2);
            var okResult = actionResult as ObjectResult;
            GroupModel group = okResult.Value as GroupModel;

            //Assert
            groupDTOs.FirstOrDefault(x => x.Id == groupId).Should().BeEquivalentTo(group);
        }

        [Test]
        public async Task GetGroupWithFreePlaces()
        {
            //Act
            ActionResult actionResult = await groupController.GetGroupWithFreePlaces(2);
            var okResult = actionResult as ObjectResult;
            GroupModelWithFreePlaces group = okResult.Value as GroupModelWithFreePlaces;

            //Assert
            groupDtoWithFreePlaces.Should().BeEquivalentTo(group);
        }

        [Test]
        public async Task GetAllGroup()
        {
            //Act
            ActionResult actionResult = await groupController.GetAll();
            var okResult = actionResult as ObjectResult;
            IEnumerable<GroupModel> groupModels = okResult.Value as IEnumerable<GroupModel>;

            //Assert
            int i = 0;
            foreach (GroupModel groupModel in groupModels)
            {
                groupModel.Should().BeEquivalentTo(groupDTOs[i]);
                i++;
            }
        }


    }
}
