using System;

namespace School.Exceptions
{
    public class InvalidCapacityException : Exception
    {
        public InvalidCapacityException(string str = "Capacity overflow") : base(str)
        {

        }
    }
}
