namespace StudentGradeManagementSystem.Models
{
    public readonly struct StudentRecord
    {
        public StudentRecord(string name, int grade)
        {
            Name = name;
            Grade = grade;
            Category = GradeCategoryHelper.FromGrade(grade);
        }

        public string Name { get; }

        public int Grade { get; }

        public GradeCategory Category { get; }

        public string CategoryName => Category.ToDisplayName();
    }
}
