using GoodPractices_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Controller
{
    class GeneralFunctions : IGeneralFunctions
    {
        private SchoolDBContext context;

        public GeneralFunctions(SchoolDBContext context)
        {
            this.context = context;
        }

        public String CheckExistence (Dictionary<String, String> input)
        {
            foreach (var pair in input)
            {
                if (pair.Key == "student" && !context.Students.Where(s=>s.Document == pair.Value).Any())
                {
                    return $"The student identified by {pair.Value} doesn't exists";
                }
                if (pair.Key == "teacher" && !context.Teachers.Where(s => s.Document == pair.Value).Any())
                {
                    return $"The Teacher identified by {pair.Value} doesn't exists";
                }
                if (pair.Key == "course" && !context.Courses.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The Course named {pair.Value} doesn't exists";
                }
                if (pair.Key == "subject" && !context.Subjects.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The Subject named {pair.Value} doesn't exists";
                }
                if (pair.Key == "foreignLanguage" && !context.ForeignLanguages.Where(s => s.Name == pair.Value).Any())
                {
                    return $"The ForeignLanguage named {pair.Value} doesn't exists";
                }
            }
            return "success";
        }
    }
}
