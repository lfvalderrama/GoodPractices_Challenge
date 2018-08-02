using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;

namespace GoodPractices_Controller
{
    class CourseController
    {
        public void CreateCourse(string document, string name, int age, ForeignLanguage language)
        {
            var context = new SchoolDBContext();
            //Course course = new Student(document, name, age, language);
            //context.Students.Add(student);
            //context.SaveChanges();
        }

        public void DeleteStudent(String document)
        {
            var context = new SchoolDBContext();
            var student = (from Student in context.Students where Student.Document == document select Student);
            if (student.Count() == 0)
            {
                Console.WriteLine($"The student identified with {document} don't exists.");
            }
            else
            {
                context.Students.Remove(student.First());
                context.SaveChanges();
            }
        }
    }
}
