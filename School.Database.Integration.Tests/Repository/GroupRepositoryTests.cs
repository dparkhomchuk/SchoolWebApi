
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using School.Database.Repository;
using School.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Database.Integration.Tests.Repository
{
    [TestFixture]
    public class GroupRepositoryTests
    {

        SchoolDataBase _context;
        List<Group> groups;


        [OneTimeSetUp]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<SchoolDataBase>()
            .UseInMemoryDatabase("Server=(localdb)\\mssqllocaldb;Database=GroupRepositoryTests;Trusted_Connection=True;ConnectRetryCount=0")
            .Options;
            _context = new SchoolDataBase(options);

            groups = new List<Group>() {
                new Group(){  Name = "SE-121", Capacity = 20 },
                new Group(){  Name = "SE-221", Capacity = 25 },
                new Group(){  Name = "SE-321", Capacity = 30 },
                new Group(){  Name = "SE-421", Capacity = 18 } };


            _context.Groups.AddRange(groups);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllGroups_In_Database()
        {
            //Arrenge
            GroupRepository groupRepository = new GroupRepository(_context);

            //Act
            IEnumerable<Group> currentGroups = await groupRepository.GetAll();

            //Assert
            groups.Should().BeEquivalentTo(currentGroups);
        }

        [Test]
        public async Task GetGroup_ById()
        {
            //Arrenge
            GroupRepository groupRepository = new GroupRepository(_context);

            //Act
            Group foundGroup = await groupRepository.Get(groups[0].Id);

            //Assert
            groups[0].Should().BeEquivalentTo(foundGroup);
        }

        [Test]
        public async Task AddGroup_To_Database()
        {
            //Arrenge
            Group group = new Group() { Name = "SE-521", Capacity = 15 };
            groups.Add(group);
            GroupRepository groupRepository = new GroupRepository(_context);

            //Act
            groupRepository.Create(group);
            await _context.SaveChangesAsync();

            //Assert
            _context.Groups.FirstOrDefault(x => x.Id == group.Id).Should().BeEquivalentTo(group);
        }




    }
}
