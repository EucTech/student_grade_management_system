namespace StudentGradeManagementSystem.Models
{
    public enum GradeCategory
    {
        Fail,
        Poor,
        Average,
        Good,
        VeryGood,
        Excellent
    }

    public static class GradeCategoryHelper
    {
        public static GradeCategory FromGrade(int grade)
        {
            if (grade >= 90) return GradeCategory.Excellent;
            if (grade >= 80) return GradeCategory.VeryGood;
            if (grade >= 70) return GradeCategory.Good;
            if (grade >= 60) return GradeCategory.Average;
            if (grade >= 50) return GradeCategory.Poor;
            return GradeCategory.Fail;
        }

        public static string ToDisplayName(this GradeCategory category)
        {
            return category switch
            {
                GradeCategory.VeryGood => "Very Good",
                _ => category.ToString()
            };
        }
    }
}
