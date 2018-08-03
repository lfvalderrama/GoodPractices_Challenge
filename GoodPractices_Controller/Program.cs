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
            ForeignLanguage language_test = new ForeignLanguage(Language.ENGLISH, "english 2", "English stuff");
            Console.WriteLine(student_controller.CreateStudent("351531", "pedrito", 10, language_test));
            Console.WriteLine(student_controller.DeleteStudent("351531"));
            Console.WriteLine(subject_controller.CreateSubject("Math 1", "Math stuff"));
            Console.WriteLine(subject_controller.CreateLanguage(Language.FRENCH, "french 2", "French stuff"));
            Console.WriteLine(subject_controller.DeleteSubject("english 1"));
        }
    }
}
