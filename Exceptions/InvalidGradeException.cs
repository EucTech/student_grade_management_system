using System;

namespace StudentGradeManagementSystem.Exceptions
{
    public class InvalidGradeException : Exception
    {
        public InvalidGradeException(string message)
            : base(message)
        {
        }
    }
}
