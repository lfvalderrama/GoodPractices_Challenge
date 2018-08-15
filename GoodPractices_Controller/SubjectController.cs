using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;

namespace GoodPractices_Controller
{
    public class SubjectController
    {
        private SchoolDBContext context;
        private Validation generalFunctions;

        public SubjectController(SchoolDBContext context)
        {
            this.context = context;
            this.generalFunctions = new Validation(context);
        }


        #region CreateSubject
        public String CreateSubject(string name, string content)
        {
            String checks = generalFunctions.CheckExistence(new Dictionary<string, string>() { { "noSubject", name } });
            if (checks != "success")
            {
                return checks;
            }
            else { 
                Subject subject = new Subject(name, content);
                context.Subjects.Add(subject);
                context.SaveChanges();
                return $"The subject {name} was created satisfactorily";
            }
        }
        #endregion

        #region CreateLanguage
        public String CreateLanguage(Language language, string name, string content)
        {
            if (!context.ForeignLanguages.Where(s => s.Name == name).Any())
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
        #endregion

        #region DeleteSubject
        public String DeleteSubject(String name)
        {
            var subject = context.Subjects.Where(s => s.Name == name);
            String checks = generalFunctions.CheckExistence(new Dictionary<string, string>() { { "subject", name } });
            if (checks != "success")
            {
                return checks;
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
        #endregion

        #region GetSubjects
        public List<string> GetSubjects()
        {
            List<string> subjectList = new List<string>();
            var subjects = context.Subjects;
            foreach (var subject in subjects)
            {
                subjectList.Add($"{subject.Name}");
            }
            return subjectList;
        }
        #endregion
    }
}
