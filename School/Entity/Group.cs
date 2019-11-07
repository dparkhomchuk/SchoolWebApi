using School.Event;
using School.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace School.Entity
{
    public class Group
    {
        internal List<IEvent> DomainEvents { get; private set; }
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int Capacity { get; internal set; }
        public virtual List<Student> Students { get; internal set; }

        public byte[] RowVersion { get; set; }

        public Group()
        {
            Students = new List<Student>();
            DomainEvents = new List<IEvent>();
        }

        public void AddStudentToGroup(Student student)
        {
            if (Capacity > Students.Count)
            {
                Students.Add(student);
                DomainEvents.Add(new StudentToGroupAdded(student.Id, Id));
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
