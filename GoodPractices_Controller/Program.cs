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

            //Console.WriteLine(student_controller.CreateStudent("351531", "pedrito", 10, language_test));
            //Console.WriteLine(student_controller.DeleteStudent("351531"));
            //Console.WriteLine(subject_controller.CreateSubject("Math 1", "Math stuff"));
            //Console.WriteLine(subject_controller.CreateLanguage(Language.FRENCH, "french 2", "French stuff"));
            //Console.WriteLine(subject_controller.DeleteSubject("english 2"));
            //Console.WriteLine(course_controller.CreateCourse( "9A", "351531", "6846684"));
            //Console.WriteLine(course_controller.CreateCourse("10A", "351541"));
            //Console.WriteLine(course_controller.DeleteCourse("11A"));
            //Console.WriteLine(teacherController.CreateTeacher("juan", "6846684", 23));
            //Console.WriteLine(teacherController.DeleteTeacher("6846684"));
            Console.WriteLine(teacherController.AddSubjectToTeacher("french 2", "6846684"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("Math 1", "6846684"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("Math 2", "6846684"));
            //Console.WriteLine(teacherController.AddSubjectToTeacher("Math 1", "68466824"));


        }
    }
}
