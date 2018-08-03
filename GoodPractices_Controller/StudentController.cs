using System;
using GoodPractices_Model;
using System.Data.Entity;
using System.Linq;


namespace GoodPractices_Controller
{
    class StudentController
    {
        public String CreateStudent(string document, string name, int age, ForeignLanguage language)
        {
            var context = new SchoolDBContext();            
            if (!context.Students.Where(x => x.Document == document).Any())
            {
                Student student = new Student(document, name, age, language);
                context.Students.Add(student);
                context.SaveChanges();
                return $"The Student {name} was created satisfactorily";
            }
            else
            {
                return ($"The student identified with {document} already exists.");
            }
        }

        public String DeleteStudent(String document)
        {
            var context = new SchoolDBContext();
            var student = context.Students.Where(x => x.Document == document);
            if (!student.Any())
            {
                return ($"The student identified with {document} don't exists.");
            }
            else
            {
                try
                {
                    context.Students.Remove(student.First());
                    context.SaveChanges();
                    return ($"The student identified with {document} was removed satisfactorily");
                }                
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return ($"The student can't be deleted, there are courses with that student as headman");
                }
            }
        } 
    }
}
