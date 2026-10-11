# Student Grade Management System

Student Grading System

## Features

- **Add a student** with a name and a grade (0 - 100)
- **Display all students** in a table showing name, grade and grade category
- **Search for a student** by name and show their grade
- **Calculate the average grade** of the class
- **Find the highest and lowest grades** using `Max()` and `Min()` on the dictionary values, including which students got them
- **Update a student's grade**
- **Remove a student** (with confirmation)
- **Full class report** with totals and how many students fall into each grade category
- Live summary panel showing the current average, highest and lowest grades

## Error Handling

| Scenario | What happens |
| --- | --- |
| Updating the grade of a student who does not exist | `StudentNotFoundException` is thrown and the user is told to add the student first |
| Searching for a student who does not exist | `StudentNotFoundException` is thrown and a "not found" message is shown |
| Grade is not a number (e.g. `abc`, `85.5`) | `InvalidGradeException` is thrown with a clear message |
| Grade is outside 0 - 100 | `InvalidGradeException` is thrown |
| Empty or invalid student name | `ArgumentException` is thrown |
| Adding a student who already exists | `DuplicateStudentException` is thrown |
| Average / highest / lowest with no students | `InvalidOperationException` is thrown and the user is asked to add students first |

Errors are shown in a message box and in the status bar at the bottom of the window.

## Grade Categories

Grade categories are represented by the `GradeCategory` enum:

| Grade | Category |
| --- | --- |
| 90 - 100 | Excellent |
| 80 - 89 | Very Good |
| 70 - 79 | Good |
| 60 - 69 | Average |
| 50 - 59 | Poor |
| 0 - 49 | Fail |

## Keyboard Shortcuts

| Key | Action |
| --- | --- |
| `Enter` (in Name box) | Move to the Grade box |
| `Enter` (in Grade box) | Add the student |
| `Enter` (in Search box) | Search for the student |
| `Ctrl + N` | Clear inputs and start a new entry |
| `Ctrl + F` | Jump to the search box |
| `Ctrl + U` | Update the grade of the student in the Name box |
| `Delete` (in the table) | Remove the selected student |
| `F5` | Show the full class report |
| `Esc` | Clear inputs |

Clicking a row in the table fills the Name and Grade boxes, which makes updating or removing a student quick.

## Project Structure

```
StudentGradeManagementSystem/
├── App.xaml / App.xaml.cs          Application entry and shared styles
├── MainWindow.xaml / .xaml.cs      User interface and event handling
├── Models/
│   ├── GradeCategory.cs            Grade category enum and helper
│   ├── StudentRecord.cs            Struct representing one student
│   └── GradeStatistics.cs          Struct holding class statistics
├── Services/
│   └── GradeBook.cs                Dictionary<string, int> storage and grade logic
└── Exceptions/
    ├── StudentNotFoundException.cs
    ├── DuplicateStudentException.cs
    └── InvalidGradeException.cs
```

## Requirements

- Windows 10 or 11
- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)

Check your installation with:

```bash
dotnet --version
```

## How to Run

1. Clone the repository:

   ```bash
   git clone https://github.com/EucTech/student_grade_management_system.git
   cd student_grade_management_system
   ```

2. Build the project:

   ```bash
   dotnet build
   ```

3. Run the application:

   ```bash
   dotnet run
   ```

You can also open the folder in Visual Studio or VS Code and run it from there.

## Usage Example

1. Type `John Doe` in the Name box, press `Enter`, type `85` and press `Enter` again to add the student.
2. Add a few more students the same way.
3. Type a name in the Search box and press `Enter` to find their grade.
4. Click **Calculate Average** or **Highest & Lowest Grades** to see the results.
5. Try typing `abc` or `150` as a grade, or searching for a name that doesn't exist, to see the error handling.
