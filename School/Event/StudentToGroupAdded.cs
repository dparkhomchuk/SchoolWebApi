namespace School.Event
{
    class StudentToGroupAdded : IEvent
    {
        public StudentToGroupAdded(int studentId, int groupdId)
        {
            StudentId = studentId;
            GroupId = groupdId;
        }

        public int StudentId { get; }
        public int GroupId { get; }
    }
}
