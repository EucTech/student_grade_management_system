using System;

namespace StudentGradeManagementSystem.Exceptions
{
    public class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(string studentName)
            : base($"Student \"{studentName}\" was not found.")
        {
            StudentName = studentName;
        }

        public string StudentName { get; }
    }
}
