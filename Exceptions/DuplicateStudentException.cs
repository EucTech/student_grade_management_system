using System;

namespace StudentGradeManagementSystem.Exceptions
{
    public class DuplicateStudentException : Exception
    {
        public DuplicateStudentException(string studentName)
            : base($"Student \"{studentName}\" already exists. Use Update Grade to change their grade.")
        {
            StudentName = studentName;
        }

        public string StudentName { get; }
    }
}
