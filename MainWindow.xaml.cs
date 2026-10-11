using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudentGradeManagementSystem.Exceptions;
using StudentGradeManagementSystem.Models;
using StudentGradeManagementSystem.Services;

namespace StudentGradeManagementSystem
{
    public partial class MainWindow : Window
    {
        private readonly GradeBook _gradeBook = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshStudentList();
            NameInput.Focus();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddStudent();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateStudentGrade();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveStudent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            ShowInfo("Inputs cleared.");
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchStudent();
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllStudents();
        }

        private void AverageButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAverage();
        }

        private void HighLowButton_Click(object sender, RoutedEventArgs e)
        {
            ShowHighestAndLowest();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowStatistics();
        }

        private void NameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GradeInput.Focus();
                e.Handled = true;
            }
        }

        private void GradeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddStudent();
                e.Handled = true;
            }
        }

        private void SearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchStudent();
                e.Handled = true;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool ctrl = Keyboard.Modifiers == ModifierKeys.Control;

            if (ctrl && e.Key == Key.N)
            {
                ClearInputs();
                NameInput.Focus();
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.F)
            {
                SearchInput.Focus();
                SearchInput.SelectAll();
                e.Handled = true;
            }
            else if (ctrl && e.Key == Key.U)
            {
                UpdateStudentGrade();
                e.Handled = true;
            }
            else if (e.Key == Key.F5)
            {
                ShowStatistics();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                ClearInputs();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete && StudentsGrid.IsKeyboardFocusWithin)
            {
                RemoveStudent();
                e.Handled = true;
            }
        }

        private void StudentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is StudentRecord student)
            {
                NameInput.Text = student.Name;
                GradeInput.Text = student.Grade.ToString();
            }
        }

        private void AddStudent()
        {
            try
            {
                int grade = GradeBook.ParseGrade(GradeInput.Text);
                _gradeBook.AddStudent(NameInput.Text, grade);

                StudentRecord student = _gradeBook.FindStudent(NameInput.Text);
                RefreshStudentList();
                ClearInputs();
                NameInput.Focus();
                ShowSuccess($"{student.Name} was added with a grade of {grade}.");
            }
            catch (InvalidGradeException ex)
            {
                ShowError(ex.Message, "Invalid Grade");
                GradeInput.Focus();
                GradeInput.SelectAll();
            }
            catch (DuplicateStudentException ex)
            {
                ShowError(ex.Message, "Student Already Exists");
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message, "Invalid Name");
                NameInput.Focus();
            }
        }

        private void UpdateStudentGrade()
        {
            try
            {
                int grade = GradeBook.ParseGrade(GradeInput.Text);
                _gradeBook.UpdateGrade(NameInput.Text, grade);

                StudentRecord student = _gradeBook.FindStudent(NameInput.Text);
                RefreshStudentList();
                SelectStudent(student.Name);
                ShowSuccess($"{student.Name}'s grade was updated to {grade}.");
            }
            catch (StudentNotFoundException ex)
            {
                ShowError($"{ex.Message} You cannot add a grade for a student who does not exist. Add the student first.",
                    "Student Not Found");
            }
            catch (InvalidGradeException ex)
            {
                ShowError(ex.Message, "Invalid Grade");
                GradeInput.Focus();
                GradeInput.SelectAll();
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message, "Invalid Name");
                NameInput.Focus();
            }
        }

        private void RemoveStudent()
        {
            string name = NameInput.Text;

            if (StudentsGrid.SelectedItem is StudentRecord selected && string.IsNullOrWhiteSpace(name))
            {
                name = selected.Name;
            }

            try
            {
                StudentRecord student = _gradeBook.FindStudent(name);

                MessageBoxResult answer = MessageBox.Show($"Are you sure you want to remove {student.Name}?",
                    "Confirm Remove", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer != MessageBoxResult.Yes)
                {
                    ShowInfo("Remove cancelled.");
                    return;
                }

                _gradeBook.RemoveStudent(student.Name);
                RefreshStudentList();
                ClearInputs();
                ShowSuccess($"{student.Name} was removed.");
            }
            catch (StudentNotFoundException ex)
            {
                ShowError(ex.Message, "Student Not Found");
            }
            catch (ArgumentException ex)
            {
                ShowError($"{ex.Message} You can also select a student from the list.", "Invalid Name");
            }
        }

        private void SearchStudent()
        {
            try
            {
                StudentRecord student = _gradeBook.FindStudent(SearchInput.Text);
                SelectStudent(student.Name);
                ShowSuccess($"Found {student.Name}: grade {student.Grade} ({student.CategoryName}).");
            }
            catch (StudentNotFoundException ex)
            {
                StudentsGrid.SelectedItem = null;
                ShowError($"{ex.Message} Check the spelling and try again.", "Student Not Found");
                SearchInput.Focus();
                SearchInput.SelectAll();
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message, "Search");
                SearchInput.Focus();
            }
        }

        private void DisplayAllStudents()
        {
            RefreshStudentList();
            SearchInput.Clear();

            if (_gradeBook.IsEmpty)
            {
                ShowInfo("No students to display yet.");
                return;
            }

            ShowInfo($"Showing all {_gradeBook.Count} student(s).");
        }

        private void ShowAverage()
        {
            try
            {
                double average = _gradeBook.GetAverageGrade();
                GradeCategory category = GradeCategoryHelper.FromGrade((int)Math.Round(average));

                AverageText.Text = average.ToString("0.00");
                ShowSuccess($"The class average is {average:0.00} ({category.ToDisplayName()}).");
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message, "No Students");
            }
        }

        private void ShowHighestAndLowest()
        {
            try
            {
                int highest = _gradeBook.GetHighestGrade();
                int lowest = _gradeBook.GetLowestGrade();
                List<string> topStudents = _gradeBook.GetStudentsWithGrade(highest);
                List<string> bottomStudents = _gradeBook.GetStudentsWithGrade(lowest);

                HighestText.Text = highest.ToString();
                LowestText.Text = lowest.ToString();

                ShowSuccess($"Highest: {highest} ({string.Join(", ", topStudents)})   " +
                            $"Lowest: {lowest} ({string.Join(", ", bottomStudents)})");
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message, "No Students");
            }
        }

        private void ShowStatistics()
        {
            try
            {
                GradeStatistics stats = _gradeBook.GetStatistics();
                Dictionary<GradeCategory, int> breakdown = _gradeBook.GetCategoryBreakdown();

                AverageText.Text = stats.Average.ToString("0.00");
                HighestText.Text = stats.Highest.ToString();
                LowestText.Text = stats.Lowest.ToString();

                var report = new StringBuilder();
                report.AppendLine($"Total students: {stats.TotalStudents}");
                report.AppendLine($"Average grade: {stats.Average:0.00}");
                report.AppendLine($"Highest grade: {stats.Highest}");
                report.AppendLine($"Lowest grade: {stats.Lowest}");
                report.AppendLine();
                report.AppendLine("Students per category:");

                foreach (var item in breakdown.OrderByDescending(pair => pair.Key))
                {
                    report.AppendLine($"  {item.Key.ToDisplayName()}: {item.Value}");
                }

                MessageBox.Show(report.ToString(), "Class Report", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowInfo("Class report generated.");
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message, "No Students");
            }
        }

        private void RefreshStudentList()
        {
            StudentsGrid.ItemsSource = _gradeBook.GetAllStudents();
            CountText.Text = $"{_gradeBook.Count} student(s)";

            if (_gradeBook.IsEmpty)
            {
                AverageText.Text = "-";
                HighestText.Text = "-";
                LowestText.Text = "-";
                return;
            }

            AverageText.Text = _gradeBook.GetAverageGrade().ToString("0.00");
            HighestText.Text = _gradeBook.GetHighestGrade().ToString();
            LowestText.Text = _gradeBook.GetLowestGrade().ToString();
        }

        private void SelectStudent(string name)
        {
            foreach (StudentRecord student in StudentsGrid.Items)
            {
                if (student.Name == name)
                {
                    StudentsGrid.SelectedItem = student;
                    StudentsGrid.ScrollIntoView(student);
                    return;
                }
            }
        }

        private void ClearInputs()
        {
            NameInput.Clear();
            GradeInput.Clear();
            StudentsGrid.SelectedItem = null;
        }

        private void ShowSuccess(string message)
        {
            StatusText.Foreground = Brushes.DarkGreen;
            StatusText.Text = message;
        }

        private void ShowInfo(string message)
        {
            StatusText.Foreground = Brushes.Black;
            StatusText.Text = message;
        }

        private void ShowError(string message, string title)
        {
            StatusText.Foreground = (Brush)FindResource("DangerBrush");
            StatusText.Text = message;
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
