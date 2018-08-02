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
            //student_controller.CreateStudent("351531", "pedrito", 10, language_test);
            //student_controller.DeleteStudent("351531");
           // subject_controller.CreateSubject("Math 1", "Math stuff");
           // subject_controller.CreateLanguage(Language.FRENCH, "french 2", "French stuff");
            subject_controller.DeleteSubject("english 1");
            Console.ReadLine();
        }
    }
}
