namespace StudentGradeManagementSystem.Models
{
    public readonly struct GradeStatistics
    {
        public GradeStatistics(double average, int highest, int lowest, int totalStudents)
        {
            Average = average;
            Highest = highest;
            Lowest = lowest;
            TotalStudents = totalStudents;
        }

        public double Average { get; }

        public int Highest { get; }

        public int Lowest { get; }

        public int TotalStudents { get; }
    }
}
