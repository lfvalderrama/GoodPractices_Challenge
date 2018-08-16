using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;

namespace GoodPractices_Controller
{
    public interface IPrinter
    {
        void PrintGradesByStudent(GradeReport gradeReport);
        void PrintGradesByTeacher(GradesByTeacher gradesByTeacher);
        void PrintMessage(string message);
        void PrintList(List<string> data);
        void PrintSubjectsOfCourses(Dictionary<string, List<string>> courses);
        void PrintMenu();
    }

    public class ConsolePrinter : IPrinter
    {
        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void PrintGradesByStudent(GradeReport gradeReport)
        {
            if (gradeReport.Error != null)
            {
                Console.WriteLine(gradeReport.Error);
            }
            else
            {
                Console.WriteLine($"Grades of the student {gradeReport.Name}");
                foreach (var period in gradeReport.Grades)
                {
                    Console.WriteLine($"\nPeriod: {period.Key}:  ");
                    foreach (var subject in period.Value)
                    {
                        Console.WriteLine($"\nSubject: {subject.Identifier}");
                        foreach (var grade in subject.Grades)
                        {
                            Console.WriteLine($"Type:  {grade.Type}     Score:  {grade.Score}");
                        }
                    }
                }
            }            
        }

        public void PrintGradesByTeacher( GradesByTeacher gradesByTeacher)
        {
            if (gradesByTeacher.Error != null)
            {
                Console.WriteLine(gradesByTeacher.Error);
            }
            else
            {
                Console.WriteLine($"Grades of the students that take class with the teacher {gradesByTeacher.TeacherName}");
                foreach (var subject in gradesByTeacher.GradesBySubject)
                {
                    Console.WriteLine($"\nSubject: {subject.Key}:  ");
                    foreach (var student in subject.Value)
                    {
                        Console.WriteLine($"\nStudent: {student.Name}");
                        foreach (var period in student.Grades)
                        {
                            Console.WriteLine($"\nPeriod: {period.Identifier}");
                            foreach (var grade in period.Grades)
                            {
                                Console.WriteLine($"Type:  {grade.Type}     Score:  {grade.Score}");
                            }
                        }
                    }
                }
            }
        }

        public void PrintList(List<string> data)
        {
            foreach (var message in data)
            {
                Console.WriteLine(message);
            }
        }

        public void PrintSubjectsOfCourses(Dictionary<string, List<string>> courses)
        {
            foreach (var course in courses)
            {
                Console.WriteLine(course.Key);
                Console.WriteLine("           SUBJECTS");
                foreach (var subject in course.Value)
                {
                    Console.WriteLine($"           {subject}");
                }
            }
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("#############MENU############\n");
            Console.WriteLine("Please select the option you want:\n" +
                              "1. Add a Student.\n" +
                              "2. Add a Teacher.\n" +
                              "3. Add a Subject.\n" +
                              "4. Add a Foreign language.\n" +
                              "5. Add a Course.\n" +
                              "6. Modify the headman of a course.\n" +
                              "7. Add a Partial Grade to a Student.\n" +
                              "8. Calculate Final grades of a student.\n" +
                              "9. List all courses.\n" +
                              "10. List all subjects.\n" +
                              "11. List all courses with its subjects.\n" +
                              "12. List all grades of the students of the subjects of a teacher.\n" +
                              "13. List Grades of a student.\n" +
                              "14. List all headmans.\n" +
                              "15. Delete a student.\n" +
                              "16. Assign subject to teacher.\n" +
                              "17. Assign student to course.\n" +
                              "18. Assign Foreign language to student.\n" +
                              "19. Assign subject to course.\n" +
                              "0. EXIT\n\n" +
                              "########################################");
        }
    }
}
