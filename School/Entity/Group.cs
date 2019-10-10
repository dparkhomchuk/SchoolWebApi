using School.Exceptions;
using System.Collections.Generic;

namespace School.Entity
{
    public class Group
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int Capacity { get; internal set; }
        public virtual List<Student> Students { get; internal set; }

        public byte[] RowVersion { get; set; }

        public Group()
        {
            Students = new List<Student>();
        }

        public void AddStudentToGroup(Student student)
        {
            if (Capacity > Students.Count)
            {
                Students.Add(student);
            }
            else
            {
                throw new InvalidCapacityException("Imposible add student, capacity overflow");
            }
        }
        public void DeleteStudentFromGroup(Student student)
        {
            Students.Remove(student);
        }
    }
}
