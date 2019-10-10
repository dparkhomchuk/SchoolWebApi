using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using School.Entity;
using School.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace School.UnitTest
{
    [TestFixture]
    public class GroupTests
    {

        private static IEnumerable<TestCaseData> StudentGroupTestCaseData
        {
            get
            {
                yield return new TestCaseData(0, new List<Student>());
                yield return new TestCaseData(1, new List<Student> { new Student() });
            }
        }

        [TestCaseSource(nameof(StudentGroupTestCaseData))]
        public void AddStudendToGroup_AddsStudentToGroup_WhenEnoughPlace(int currentCapacity, List<Student> initialStudentList)
        {
            // Arrange
            var group = new Group();
            group.Capacity = currentCapacity + 1;
            group.Students = initialStudentList;

            var newStudent = new Student() { Id = initialStudentList.Count + 1 };
            var expectedStudents = new List<Student>(initialStudentList.AsEnumerable());
            expectedStudents.Add(newStudent);

            // Act
            group.AddStudentToGroup(newStudent);

            // Assert
            group.Students.Should().BeEquivalentTo(expectedStudents);
        }


        [TestCaseSource(nameof(StudentGroupTestCaseData))]
        [ExpectedException(typeof(InvalidCapacityException))]
        public void AddStudentToGroup_AddsStudentToGroup_WhenNotEnoughPlace(int currentCapacity, List<Student> initialStudentList)
        {
            //Arrenge
            Group group = new Group();
            group.Capacity = currentCapacity;
            group.Students = initialStudentList;

            //Act
            //Assert
            NUnit.Framework.Assert.Throws<InvalidCapacityException>(() => group.AddStudentToGroup(new Student()));
        }



    }
}
