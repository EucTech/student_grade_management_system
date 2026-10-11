using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradeManagementSystem.Exceptions;
using StudentGradeManagementSystem.Models;

namespace StudentGradeManagementSystem.Services
{
    public class GradeBook
    {
        public const int MinGrade = 0;
        public const int MaxGrade = 100;

        private readonly Dictionary<string, int> _students = new(StringComparer.OrdinalIgnoreCase);

        public int Count => _students.Count;

        public bool IsEmpty => _students.Count == 0;

        public void AddStudent(string name, int grade)
        {
            string studentName = NormalizeName(name);
            ValidateGrade(grade);

            if (_students.ContainsKey(studentName))
            {
                throw new DuplicateStudentException(studentName);
            }

            _students.Add(studentName, grade);
        }

        public void UpdateGrade(string name, int grade)
        {
            string studentName = NormalizeName(name);
            ValidateGrade(grade);

            if (!_students.ContainsKey(studentName))
            {
                throw new StudentNotFoundException(studentName);
            }

            _students[studentName] = grade;
        }

        public void RemoveStudent(string name)
        {
            string studentName = NormalizeName(name);

            if (!_students.Remove(studentName))
            {
                throw new StudentNotFoundException(studentName);
            }
        }

        // Student records
        public StudentRecord FindStudent(string name)
        {
            string studentName = NormalizeName(name);

            foreach (var entry in _students)
            {
                if (string.Equals(entry.Key, studentName, StringComparison.OrdinalIgnoreCase))
                {
                    return new StudentRecord(entry.Key, entry.Value);
                }
            }

            throw new StudentNotFoundException(studentName);
        }


        // Student list
        public List<StudentRecord> GetAllStudents()
        {
            return _students
                .Select(entry => new StudentRecord(entry.Key, entry.Value))
                .OrderBy(student => student.Name)
                .ToList();
        }

        public double GetAverageGrade()
        {
            EnsureHasStudents();
            return _students.Values.Average();
        }

        public int GetHighestGrade()
        {
            EnsureHasStudents();
            return _students.Values.Max();
        }

        public int GetLowestGrade()
        {
            EnsureHasStudents();
            return _students.Values.Min();
        }

        public List<string> GetStudentsWithGrade(int grade)
        {
            return _students
                .Where(entry => entry.Value == grade)
                .Select(entry => entry.Key)
                .OrderBy(name => name)
                .ToList();
        }

        public GradeStatistics GetStatistics()
        {
            return new GradeStatistics(GetAverageGrade(), GetHighestGrade(), GetLowestGrade(), Count);
        }

        public Dictionary<GradeCategory, int> GetCategoryBreakdown()
        {
            var breakdown = new Dictionary<GradeCategory, int>();

            foreach (GradeCategory category in Enum.GetValues(typeof(GradeCategory)))
            {
                breakdown[category] = 0;
            }

            foreach (int grade in _students.Values)
            {
                breakdown[GradeCategoryHelper.FromGrade(grade)]++;
            }

            return breakdown;
        }

        public void Clear()
        {
            _students.Clear();
        }

        public static int ParseGrade(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new InvalidGradeException("Please enter a grade.");
            }

            if (!int.TryParse(input.Trim(), out int grade))
            {
                throw new InvalidGradeException($"\"{input.Trim()}\" is not a valid grade. Grades must be whole numbers.");
            }

            ValidateGrade(grade);
            return grade;
        }

        private static void ValidateGrade(int grade)
        {
            if (grade < MinGrade || grade > MaxGrade)
            {
                throw new InvalidGradeException($"Grade must be between {MinGrade} and {MaxGrade}. You entered {grade}.");
            }
        }

        private static string NormalizeName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Please enter a student name.");
            }

            string trimmed = string.Join(" ", name.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            if (!trimmed.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\''))
            {
                throw new ArgumentException("Student name can only contain letters, spaces, hyphens and apostrophes.");
            }

            return trimmed;
        }

        private void EnsureHasStudents()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("There are no students yet. Add a student first.");
            }
        }
    }
}
