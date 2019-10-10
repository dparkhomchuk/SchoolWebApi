namespace School.Entity
{
    public class Student
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string Surname { get; internal set; }
        public int Age { get; internal set; }

        public int? GroupId { get; internal set; }
        public virtual Group Group { get; private set; }
    }
}
