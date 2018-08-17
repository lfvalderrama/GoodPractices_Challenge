using GoodPractices_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Controller
{
    public class Validation : IValidation
    {
        private ISchoolDBContext _context;

        public Validation(ISchoolDBContext context)
        {
            _context = context;
        }

        public String CheckExistence (Dictionary<String, String> input)
        {
            foreach (var pair in input)
            {
                if (pair.Key == "student" && !_context.Students.Where(s=>s.Document == pair.Value).Any())
                {
                    return $"The student identified by {pair.Value} doesn't exists";
                }
                if (pair.Key == "teacher" && !_context.Teachers.Where(s => s.Document == pair.Value).Any())
                {
                    return $"The Teacher identified by {pair.Value} doesn't exists";
                }
                if (pair.Key == "course" && !_context.Courses.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The Course named {pair.Value} doesn't exists";
                }
                if (pair.Key == "subject" && !_context.Subjects.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The Subject named {pair.Value} doesn't exists";
                }
                if (pair.Key == "foreignLanguage" && !_context.ForeignLanguages.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The ForeignLanguage named {pair.Value} doesn't exists";
                }
                if (pair.Key == "noCourse" && _context.Courses.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The Course named {pair.Value} already exists";
                }
                if (pair.Key == "noStudent" && _context.Students.Where(s => s.Document == pair.Value).Any())
                {
                    return $"The student identified by {pair.Value} already exists";
                }
                if (pair.Key == "noSubject" && _context.Subjects.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The Subject named {pair.Value} already exists";
                }
                if (pair.Key == "noTeacher" && _context.Teachers.Where(s => s.Document == pair.Value).Any())
                {
                    return $"The Teacher identified by {pair.Value} already exists";
                }
                if (pair.Key == "noForeignLanguage" && _context.ForeignLanguages.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The ForeignLanguage named {pair.Value} already exists";
                }
            }
            return "success";
        }
    }
}
