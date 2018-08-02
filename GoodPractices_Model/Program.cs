using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Model
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new SchoolDBContext();
            ForeignLanguage language_test = new ForeignLanguage(Language.FRENCH, "French 1", "cosas de frances");
          //  Student student_test = new Student(language_test, "123154685", "pepito", 10);
            //Student student_test2 = new Student(language_test, "12315468485", "pepito 2", 11);
            Subject subject_test = new Subject("Matematicas 1", "sumas");
            //Course course_test = new Course(new List<Student>() {  student_test2 }, new List<Subject>() { subject_test }, student_test2);
            //Course course_test2 = new Course(new List<Student>() { student_test }, new List<Subject>() { subject_test }, student_test);            
                
               // context.Courses.Add(course_test2);
                context.SaveChanges();
            Console.ReadLine();
        }
    }
}
