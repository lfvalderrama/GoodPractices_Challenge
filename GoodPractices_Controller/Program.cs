﻿using GoodPractices_Model;
using System;
using Autofac;

namespace GoodPractices_Controller
{
    public class Program
    {

        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {

            // Create your builder.
            var builder = new ContainerBuilder();

            // Usually you're only interested in exposing the type
            // via its interface:
            builder.RegisterType<Validation>().As<IValidation>();
            builder.RegisterType<Printer>().As<IPrinter>();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                //var generalFunctions = scope.Resolve<IValidation>();

                var printer = scope.Resolve<IPrinter>();
                StudentController student_controller = new StudentController(new SchoolDBContext());
                SubjectController subject_controller = new SubjectController(new SchoolDBContext());
                CourseController course_controller = new CourseController(new SchoolDBContext());
                TeacherController teacherController = new TeacherController(new SchoolDBContext());
                GradeController gradeController = new GradeController(new SchoolDBContext());
                AdministratorController administratorController = new AdministratorController(new SchoolDBContext());

                string option = "100";

                while (option != "0")
                {
                    printer.PrintMenu();
                    option = Console.ReadLine();
                    Console.Clear();
                    switch (option)
                    {
                        case "0":
                            break;
                        case "1":
                            printer.PrintMessage("1. Add a Student.\n");
                            printer.PrintMessage("Document:");
                            string document = Console.ReadLine();
                            printer.PrintMessage("Name:");
                            string name = Console.ReadLine();
                            printer.PrintMessage("Age:");
                            int age = Convert.ToInt32(Console.ReadLine());
                            printer.PrintMessage(student_controller.CreateStudent(document, name, age));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "2":
                            printer.PrintMessage("2. Add a Teacher.\n");
                            printer.PrintMessage("Document:");
                            document = Console.ReadLine();
                            printer.PrintMessage("Name:");
                            name = Console.ReadLine();
                            printer.PrintMessage("Age:");
                            age = Convert.ToInt32(Console.ReadLine());
                            printer.PrintMessage(teacherController.CreateTeacher(document, name, age));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "3":
                            printer.PrintMessage("3. Add a Subject.\n");
                            printer.PrintMessage("Name:");
                            name = Console.ReadLine();
                            printer.PrintMessage("Content:");
                            string content = Console.ReadLine();
                            printer.PrintMessage(subject_controller.CreateSubject(name, content));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "4":
                            printer.PrintMessage("4. Add a Foreign language.\n");
                            printer.PrintMessage("Name:");
                            name = Console.ReadLine();
                            printer.PrintMessage("Content:");
                            content = Console.ReadLine();
                            printer.PrintMessage("Select the language:\n" +
                                              "ENGLISH = 1\n" +
                                              "SPANISH = 2\n" +
                                              "PORTUGUESE = 3\n" +
                                              "FRENCH = 4");
                            int language = Convert.ToInt32(Console.ReadLine());
                            printer.PrintMessage(subject_controller.CreateLanguage((Language) (language), name, content));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "5":
                            printer.PrintMessage("5. Add a Course.\n");
                            printer.PrintMessage("Course name:");
                            name = Console.ReadLine();
                            printer.PrintMessage("Headman Document:");
                            string headmanDocument = Console.ReadLine();
                            printer.PrintMessage("Teacher Document:");
                            string teacherDocument = Console.ReadLine();
                            printer.PrintMessage(course_controller.CreateCourse(name, headmanDocument, teacherDocument));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "6":
                            printer.PrintMessage("6. Modify the headman of a course.\n");
                            printer.PrintMessage("Course name:");
                            string courseName = Console.ReadLine();
                            printer.PrintMessage("Headman Document:");
                            headmanDocument = Console.ReadLine();
                            printer.PrintMessage(course_controller.ReasignHeadman(courseName, headmanDocument));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "7":
                            printer.PrintMessage("7. Add a Partial Grade to a Student.\n");
                            printer.PrintMessage("Student document:");
                            document = Console.ReadLine();
                            printer.PrintMessage("Period:");
                            string period = Console.ReadLine();
                            printer.PrintMessage("Select the type:\n" +
                                              "PARTIAL1 = 1\n" +
                                              "PARTIAL2 = 2\n" +
                                              "PARTIAL3 = 3");
                            int type = Convert.ToInt32(Console.ReadLine());
                            printer.PrintMessage("Subject name:");
                            string subjectName = Console.ReadLine();
                            printer.PrintMessage("Score:");
                            double score = Convert.ToDouble(Console.ReadLine());
                            printer.PrintMessage(gradeController.AddPartialGradeToStudent(period, (float) score,
                                subjectName, (GradeType) type, document));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "8":
                            printer.PrintMessage("8. Calculate Final grades of a student.\n");
                            printer.PrintMessage("Student document:");
                            document = Console.ReadLine();
                            printer.PrintMessage("Period:");
                            period = Console.ReadLine();
                            printer.PrintMessage(gradeController.CalculateFinalGradeToStudent(period, document));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "9":
                            printer.PrintMessage("9. List all courses.\n\nCOURSES:");
                            printer.PrintList(course_controller.GetCourses());
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "10":
                            printer.PrintMessage("10. List all subjects.\n\nSUBJECTS:");
                            printer.PrintList(subject_controller.GetSubjects());
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "11":
                            printer.PrintMessage("11. List all courses with its subjects.\n\nCOURSES" );
                            printer.PrintSubjectsOfCourses
                                (course_controller.GetSUbjectsByCourse());
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "12":
                            printer.PrintMessage("12. List all grades of the students of the subjects of a teacher.\n\nTeacher Document:");
                            teacherDocument = Console.ReadLine();
                            printer.PrintGradesByTeacher(teacherController.GetGradesOfStudentsByTeacher(teacherDocument));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "13":
                            printer.PrintMessage("13. List Grades of a student.\n\n Student Document: ");
                            string studentDocument = Console.ReadLine();
                            printer.PrintGradesByStudent(student_controller.GetGradesByPeriod(studentDocument));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "14":
                            printer.PrintMessage("14. List all headmans.\n");
                            printer.PrintList(course_controller.GetHeadmans());
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "15":
                            printer.PrintMessage("15. Delete a student.\n");
                            printer.PrintMessage("Student Document:");
                            studentDocument = Console.ReadLine();
                            printer.PrintMessage(student_controller.DeleteStudent(studentDocument));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "16":
                            printer.PrintMessage("16.Assign subject to teacher.\n");
                            printer.PrintMessage("Teacher Document:");
                            teacherDocument = Console.ReadLine();
                            printer.PrintMessage("Subject name");
                            subjectName = Console.ReadLine();
                            printer.PrintMessage(administratorController.AddSubjectToTeacher(subjectName, teacherDocument));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "17":
                            printer.PrintMessage("17. Assign student to course.\n");
                            printer.PrintMessage("Student Document:");
                            studentDocument = Console.ReadLine();
                            printer.PrintMessage("Course name");
                            courseName = Console.ReadLine();
                            printer.PrintMessage(administratorController.AddStudentToCourse(studentDocument, courseName));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "18":
                            printer.PrintMessage("18. Assign Foreign language to student.\n");
                            printer.PrintMessage("Student Document:");
                            studentDocument = Console.ReadLine();
                            printer.PrintMessage("Foreign Language subject name:");
                            subjectName = Console.ReadLine();
                            printer.PrintMessage(student_controller.AssignForeignLanguage(studentDocument, subjectName));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "19":
                            printer.PrintMessage("19. Assign subject to course.\n");
                            printer.PrintMessage("Subject name:");
                            subjectName = Console.ReadLine();
                            printer.PrintMessage("Course Name:");
                            courseName = Console.ReadLine();
                            printer.PrintMessage(administratorController.AddSubjectToCourse(subjectName, courseName));
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        default:
                            printer.PrintMessage("Ingrese una opcion valida.");
                            printer.PrintMessage("Press a key to continue....");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }
}


//TODO print Class