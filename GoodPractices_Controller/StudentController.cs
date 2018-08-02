using System;
using GoodPractices_Model;
using System.Data.Entity;
using System.Linq;


namespace GoodPractices_Controller
{
    class StudentController
    {
        public void CreateStudent(string document, string name, int age, ForeignLanguage language)
        {
            var context = new SchoolDBContext();
            int numStudentsDocument = (from Student in context.Students where Student.Document == document select Student).Count();
            
            if (context.Students.Where(x => x.Document == document).Any())
            {
                Student student = new Student(document, name, age, language);
                context.Students.Add(student);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"The student identified with {document} already exists.");
            }
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
