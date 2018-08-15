using GoodPractices_Model;
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
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                //var generalFunctions = scope.Resolve<IValidation>();


                StudentController student_controller = new StudentController(new SchoolDBContext());
                SubjectController subject_controller = new SubjectController(new SchoolDBContext());
                CourseController course_controller = new CourseController(new SchoolDBContext());
                TeacherController teacherController = new TeacherController(new SchoolDBContext());
                GradeController gradeController = new GradeController(new SchoolDBContext());
                AdministratorController administratorController = new AdministratorController(new SchoolDBContext());

                string option = "100";

                while (option != "0")
                {
                    Console.Clear();
                    Console.WriteLine("#############MENU############");
                    Console.WriteLine();
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
                    option = Console.ReadLine();
                    Console.Clear();
                    switch (option)
                    {
                        case "0":
                            break;
                        case "1":
                            Console.WriteLine("1. Add a Student.\n");
                            Console.WriteLine("Document:");
                            string document = Console.ReadLine();
                            Console.WriteLine("Name:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Age:");
                            int age = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(student_controller.CreateStudent(document, name, age));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "2":
                            Console.WriteLine("2. Add a Teacher.\n");
                            Console.WriteLine("Document:");
                            document = Console.ReadLine();
                            Console.WriteLine("Name:");
                            name = Console.ReadLine();
                            Console.WriteLine("Age:");
                            age = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(teacherController.CreateTeacher(document, name, age));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "3":
                            Console.WriteLine("3. Add a Subject.\n");
                            Console.WriteLine("Name:");
                            name = Console.ReadLine();
                            Console.WriteLine("Content:");
                            string content = Console.ReadLine();
                            Console.WriteLine(subject_controller.CreateSubject(name, content));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "4":
                            Console.WriteLine("4. Add a Foreign language.\n");
                            Console.WriteLine("Name:");
                            name = Console.ReadLine();
                            Console.WriteLine("Content:");
                            content = Console.ReadLine();
                            Console.WriteLine("Select the language:\n" +
                                              "ENGLISH = 1\n" +
                                              "SPANISH = 2\n" +
                                              "PORTUGUESE = 3\n" +
                                              "FRENCH = 4");
                            int language = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(subject_controller.CreateLanguage((Language) (language), name, content));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "5":
                            Console.WriteLine("5. Add a Course.\n");
                            Console.WriteLine("Course name:");
                            name = Console.ReadLine();
                            Console.WriteLine("Headman Document:");
                            string headmanDocument = Console.ReadLine();
                            Console.WriteLine("Teacher Document:");
                            string teacherDocument = Console.ReadLine();
                            Console.WriteLine(course_controller.CreateCourse(name, headmanDocument, teacherDocument));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "6":
                            Console.WriteLine("6. Modify the headman of a course.\n");
                            Console.WriteLine("Course name:");
                            string courseName = Console.ReadLine();
                            Console.WriteLine("Headman Document:");
                            headmanDocument = Console.ReadLine();
                            Console.WriteLine(course_controller.ReasignHeadman(courseName, headmanDocument));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "7":
                            Console.WriteLine("7. Add a Partial Grade to a Student.\n");
                            Console.WriteLine("Student document:");
                            document = Console.ReadLine();
                            Console.WriteLine("Period:");
                            string period = Console.ReadLine();
                            Console.WriteLine("Select the type:\n" +
                                              "PARTIAL1 = 1\n" +
                                              "PARTIAL2 = 2\n" +
                                              "PARTIAL3 = 3");
                            int type = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Subject name:");
                            string subjectName = Console.ReadLine();
                            Console.WriteLine("Score:");
                            double score = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine(gradeController.AddPartialGradeToStudent(period, (float) score,
                                subjectName, (GradeType) type, document));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "8":
                            Console.WriteLine("8. Calculate Final grades of a student.\n");
                            Console.WriteLine("Student document:");
                            document = Console.ReadLine();
                            Console.WriteLine("Period:");
                            period = Console.ReadLine();
                            Console.WriteLine(gradeController.CalculateFinalGradeToStudent(period, document));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "9":
                            Console.WriteLine("9. List all courses.\n");
                            course_controller.GetCourses();
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "10":
                            Console.WriteLine("10. List all subjects.\n");
                            subject_controller.GetSubjects();
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "11":
                            Console.WriteLine("11. List all courses with its subjects.\n");
                            course_controller.GetSUbjectsByCourse();
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "12":
                            Console.WriteLine("12. List all grades of the students of the subjects of a teacher.\n");
                            Console.WriteLine("Teacher Document:");
                            teacherDocument = Console.ReadLine();
                            teacherController.GetGradesOfStudentsByTeacher(teacherDocument);
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "13":
                            Console.WriteLine("13. List Grades of a student.\n");
                            Console.WriteLine("Student Document:");
                            string studentDocument = Console.ReadLine();
                            student_controller.GetGradesByPeriod(studentDocument);
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "14":
                            Console.WriteLine("14. List all headmans.\n");
                            course_controller.GetHeadmans();
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "15":
                            Console.WriteLine("15. Delete a student.\n");
                            Console.WriteLine("Student Document:");
                            studentDocument = Console.ReadLine();
                            Console.WriteLine(student_controller.DeleteStudent(studentDocument));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "16":
                            Console.WriteLine("16.Assign subject to teacher.\n");
                            Console.WriteLine("Teacher Document:");
                            teacherDocument = Console.ReadLine();
                            Console.WriteLine("Subject name");
                            subjectName = Console.ReadLine();
                            Console.WriteLine(administratorController.AddSubjectToTeacher(subjectName, teacherDocument));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "17":
                            Console.WriteLine("17. Assign student to course.\n");
                            Console.WriteLine("Student Document:");
                            studentDocument = Console.ReadLine();
                            Console.WriteLine("Course name");
                            courseName = Console.ReadLine();
                            Console.WriteLine(administratorController.AddStudentToCourse(studentDocument, courseName));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "18":
                            Console.WriteLine("18. Assign Foreign language to student.\n");
                            Console.WriteLine("Student Document:");
                            studentDocument = Console.ReadLine();
                            Console.WriteLine("Foreign Language subject name:");
                            subjectName = Console.ReadLine();
                            Console.WriteLine(student_controller.AssignForeignLanguage(studentDocument, subjectName));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        case "19":
                            Console.WriteLine("19. Assign subject to course.\n");
                            Console.WriteLine("Subject name:");
                            subjectName = Console.ReadLine();
                            Console.WriteLine("Course Name:");
                            courseName = Console.ReadLine();
                            Console.WriteLine(administratorController.AddSubjectToCourse(subjectName, courseName));
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                        default:
                            Console.WriteLine("Ingrese una opcion valida.");
                            Console.WriteLine("Press a key to continue....");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }
}


//TODO print Class