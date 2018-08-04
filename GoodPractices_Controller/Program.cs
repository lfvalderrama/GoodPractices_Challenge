using GoodPractices_Model;
using System;

namespace GoodPractices_Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentController student_controller = new StudentController();
            SubjectControler subject_controller = new SubjectControler();
            CourseController course_controller = new CourseController();
            TeacherController teacherController = new TeacherController();
            GradeController gradeController = new GradeController();

            //Console.WriteLine(student_controller.CreateStudent("351531", "pedrito", 10));
            //Console.WriteLine(student_controller.DeleteStudent("351531"));
            //Console.WriteLine(subject_controller.CreateSubject("Math 1", "Math stuff"));
            //Console.WriteLine(subject_controller.CreateLanguage(Language.FRENCH, "french 2", "French stuff"));
            //Console.WriteLine(subject_controller.DeleteSubject("english 2"));
            //Console.WriteLine(teacherController.CreateTeacher("pedro", "6846685", 28));
            //Console.WriteLine(course_controller.CreateCourse( "9A", "351531", "6846685"));
            //Console.WriteLine(course_controller.DeleteCourse("11A"));
            //Console.WriteLine(student_controller.CreateStudent("3515328", "kevinsito", 11));
            //Console.WriteLine(course_controller.CreateCourse("10B", "351532", "6846685"));
            //Console.WriteLine(teacherController.DeleteTeacher("6846684"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("french 2", "6846685"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("Math 1", "6846685"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("Math 2", "6846685"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("Math 1", "6846685"));

            //Console.WriteLine(course_controller.AddStudentToCourse("3515328", "9A"));
            //course_controller.GetHeadmans();
            //Console.WriteLine(course_controller.AddSubjectToCourse("Math 1", "9A"));
            //Console.WriteLine(course_controller.AddSubjectToCourse("Math 1", "9A"));
            //Console.WriteLine(course_controller.AddSubjectToCourse("Math 1", "15B"));
            //course_controller.GetCourses();
            //subject_controller.GetSubjects();
            //course_controller.GetSUbjectsByCourse();

            //Console.WriteLine(course_controller.ReasignHeadman("9A", "3515328"));
            //Console.WriteLine(course_controller.ReasignHeadman("9A", "35153289"));
            //Console.WriteLine(course_controller.ReasignHeadman("9A", "351531"));

            //Console.WriteLine(student_controller.AssignForeignLanguage("351531", "french 2"));
            // Console.WriteLine(student_controller.AssignForeignLanguage("351531", "Math 1"));

            //Console.WriteLine(gradeController.AddPartialGradeToStudent("2018-1", 4.5f , "french 2", GradeType.PARTIAL2, "351531"));
            //Console.WriteLine(gradeController.CalculateFinalGradeToStudent("2018-1", "3515328"));

            //student_controller.GetGradesByPeriod("351531");
            teacherController.GetGradesOfStudentsByTeacher("6846685");
        }
    }
}
