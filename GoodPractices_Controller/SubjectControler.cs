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
        public String CreateSubject(string name, string content)
        {
            var context = new SchoolDBContext();
            if (!context.Subjects.Where(s => s.Name ==  name).Any())
            {
                Subject subject = new Subject(name,content);
                context.Subjects.Add(subject);
                context.SaveChanges();
                return $"The subject {name} was created satisfactorily";
            }
            else
            {
                return $"The subject named {name} already exists.";
            }
        }

        public String CreateLanguage(Language language, string name, string content)
        {
            var context = new SchoolDBContext();
            if (!context.Subjects.Where(s => s.Name == name).Any())
            {
                ForeignLanguage new_language = new ForeignLanguage(language, name, content);
                context.ForeignLanguages.Add(new_language);
                context.SaveChanges();
                return $"The subject {name} was created satisfactorily";

            }
            else
            {
                return $"The subject named {name} already exists.";
            }
        }

        public String DeleteSubject(String name)
        {
            var context = new SchoolDBContext();
            var subject = context.Subjects.Where(s => s.Name == name);
            if (!subject.Any())
            {
                return ($"The subject named {name} don't exists.");
            }
            else
            {
                try
                {
                    context.Subjects.Remove(subject.First());
                    context.SaveChanges();
                    return $"The subject {name} was deleted satisfactorily";

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return ($"The subject can't be deleted, there are students or teachers with that subject");
                }
                
            }
        }
    }
}
