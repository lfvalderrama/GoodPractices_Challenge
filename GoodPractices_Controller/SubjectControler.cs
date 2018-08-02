using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;

namespace GoodPractices_Controller
{
    class SubjectControler
    {
        public void CreateSubject(string name, string content)
        {
            var context = new SchoolDBContext();
            var SubjectsName = (from Subject in context.Subjects where Subject.Name == name select Subject);
            if (SubjectsName.Count() == 0)
            {
                Subject subject = new Subject(name,content);
                context.Subjects.Add(subject);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"The subject named {name} already exists.");
            }
        }

        public void CreateLanguage(Language language, string name, string content)
        {
            var context = new SchoolDBContext();
            var SubjectsName = (from Subject in context.Subjects where Subject.Name == name select Subject);
            if (SubjectsName.Count() == 0)
            {
                ForeignLanguage new_language = new ForeignLanguage(language, name, content);
                context.ForeignLanguages.Add(new_language);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine($"The subject named {name} already exists.");
            }
        }

        public void DeleteSubject(String name)
        {
            var context = new SchoolDBContext();
            var subject = (from Subject in context.Subjects where Subject.Name == name select Subject).First();
            if (subject == null)
            {
                Console.WriteLine($"The subject named {name} don't exists.");
            }
            else
            {

                try
                {
                    //var studentsQuantity = (from Student in context.Students where Student.ForeignLanguaje.Id == subject.Id select Student.Id).Count();
                    //var courseQuantity = (from Course in context.Courses
                    // where Course.Subjects.Contains(subject) select Course.Id).Count();
                    //if (studentsQuantity == 0 && courseQuantity == 0)
                    //{
                    context.Subjects.Remove(subject);
                    context.SaveChanges();
                }
                catch(System.Data.Entity.Infrastructure.DbUpdateException dbe)
                {
                    Console.WriteLine(dbe.InnerException.InnerException.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"The subject can't be deleted, there are students or teachers with that subject");
                }
                //}
                
            }
        }
    }
}
