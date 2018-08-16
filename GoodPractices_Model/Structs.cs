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
        public Dictionary<string, List<GradesBy_>> Grades;
        public string Error;

        public GradeReport(string name, Dictionary<string, List<GradesBy_>> grades, string error)
        {
            Name = name;
            Grades = grades;
            Error = error;
        }
    }

    public struct GradeByStudent
    {
        public string Name;
        public  List<GradesBy_> Grades;

        public GradeByStudent(string name, List<GradesBy_> grades)
        {
            Name = name;
            Grades = grades;
        }
    }

    public struct GradesBy_
    {
        public string Identifier;
        public List<Grade> Grades;
        public GradesBy_(string subject, List<Grade> gradesList)
        {
            Identifier = subject;
            Grades = gradesList;
        }
    }

    public struct GradesByTeacher
    {
        public string TeacherName;
        public Dictionary<string, List<GradeByStudent>> GradesBySubject;
        public string Error;

        public GradesByTeacher(string teacherName, Dictionary<string, List<GradeByStudent>> gradesBySubject, string error)
        {
            this.TeacherName = teacherName;
            this.GradesBySubject = gradesBySubject;
            Error = error;
        }
    }
}
