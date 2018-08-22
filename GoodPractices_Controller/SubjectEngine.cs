using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;

namespace GoodPractices_Engine
{
    public class SubjectEngine
    {
        private ISchoolDBContext _context;
        private IValidation _validator;

        public SubjectEngine(ISchoolDBContext context, IValidation validation)
        {
            _context = context;
            _validator = validation;
        }


        #region CreateSubject
        public String CreateSubject(string name, string content)
        {
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "noSubject", name } });
            if (checks != "success")
            {
                return checks;
            }
            else { 
                Subject subject = new Subject(name, content);
                _context.Subjects.Add(subject);
                _context.SaveChanges();
                return $"The subject {name} was created satisfactorily";
            }
        }
        #endregion

        #region CreateLanguage
        public String CreateLanguage(Language language, string name, string content)
        {
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "noForeignLanguage", name } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                ForeignLanguage new_language = new ForeignLanguage(language, name, content);
                _context.ForeignLanguages.Add(new_language);
                _context.SaveChanges();
                return $"The subject {name} was created satisfactorily";
            }
        }
        #endregion

        #region DeleteSubject
        public String DeleteSubject(String name)
        {
            var subject = _context.Subjects.Where(s => s.Name == name);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "subject", name } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    _context.Subjects.Remove(subject.First());
                    _context.SaveChanges();
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
            var subjects = _context.Subjects;
            foreach (var subject in subjects)
            {
                subjectList.Add($"{subject.Name}");
            }
            return subjectList;
        }
        #endregion
    }
}
