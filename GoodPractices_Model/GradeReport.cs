using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodPractices_Model
{
    public struct GradeReport
    {
        public string Name;
        public Dictionary<string, List<GradesBySubject>> Grades;
        public string Error;

        public GradeReport(string name, Dictionary<string, List<GradesBySubject>> grades, string error)
        {
            Name = name;
            Grades = grades;
            Error = error;
        }
    }

    public struct GradesBySubject
    {
        public string subjectName;
        public List<Grade> grades;
        public GradesBySubject(string subject, List<Grade> gradesList)
        {
            subjectName = subject;
            grades = gradesList;
        }
    }

    public struct GradesByTeacher
    {
        public string teacherName;
        public Dictionary<string, List<GradeReport>> gradesBySubject;
        public string Error;

        public GradesByTeacher(string teacherName, Dictionary<string, List<GradeReport>> gradesBySubject, string error)
        {
            this.teacherName = teacherName;
            this.gradesBySubject = gradesBySubject;
            Error = error;
        }
    }
}
