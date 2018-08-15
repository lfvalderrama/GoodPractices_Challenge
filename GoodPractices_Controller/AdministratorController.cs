using GoodPractices_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    class AdministratorController
    {
        private SchoolDBContext context;
        private Validation _generalFunctions;

        public AdministratorController(SchoolDBContext context)
        {
            this.context = context;
            this._generalFunctions = new Validation(context);
        }

        #region AddStudentToCourse
        public string AddStudentToCourse(string studentDocument, string courseName)
        {
            var course = context.Courses.Include(c => c.Students).Where(c => c.Name == courseName);
            var student = context.Students.Where(s => s.Document == studentDocument);
            String checks = _generalFunctions.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument }, { "course", courseName } });
            if (checks != "success")
            {
                return checks;
            }
            if (!context.Courses.Where(c => c.Headman.Document == studentDocument).Any())
            {
                if (!course.First().Students.Contains(student.First()))
                {
                    if (course.First().Students.Count() < 30)
                    {
                        course.First().Students.Add(student.First());
                        context.SaveChanges();
                        return $"The student identified by {studentDocument} was assigned to {courseName} satisfactorily";
                    }
                    else
                    {
                        return $"The course {courseName} can't have more than 30 students";
                    }
                }
                else
                {
                    return $"The student identified by {studentDocument} is already in {courseName}";
                }
            }
            else
            {
                return $"The student identified by {studentDocument} is headman in {context.Courses.Where(c => c.Headman.Document == studentDocument).First().Name}";
            }
        }
        #endregion

        #region AddSubjectToCourse
        public String AddSubjectToCourse(String subjectName, String courseName)
        {
            {
                var course = context.Courses.Include(c => c.Subjects).Where(c => c.Name == courseName);
                var subject = context.Subjects.Where(s => s.Name == subjectName);
                String checks = _generalFunctions.CheckExistence(new Dictionary<string, string>() { { "subject", subjectName }, { "course", courseName } });
                if (checks != "success")
                {
                    return checks;
                }
                if (subject.First().GetType() == typeof(ForeignLanguage))
                {
                    return $"The subject {subjectName} is a foreign language and can't be assigned to a course.";
                }
                if (!course.First().Subjects.Contains(subject.First()))
                {
                    course.First().Subjects.Add(subject.First());
                    context.SaveChanges();
                    return $"The subject {subjectName} was assigned to {courseName} satisfactorily";
                }
                else
                {
                    return $"The subject {subjectName} is already in {courseName}";
                }
            }
        }
        #endregion

        #region AddSubjectToTeacher
        public String AddSubjectToTeacher(String subjectName, String teacherDocument)
        {
            var subject = context.Subjects.Include(s => s.Teachers).Where(s => s.Name == subjectName);
            var teacher = context.Teachers.Where(t => t.Document == teacherDocument);
            String checks = _generalFunctions.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument }, { "subject", subjectName } });
            if (checks != "success")
            {
                return checks;
            }
            if (!subject.First().Teachers.Contains(teacher.First()))
            {
                subject.First().Teachers.Add(teacher.First());
                context.SaveChanges();
                return $"The subject {subjectName} was assigned to the teacher identified by {teacherDocument} satisfactorily";
            }
            else
            {
                return $"The teacher identified by {teacherDocument} already has the subject {subjectName}";
            }
        }
        #endregion
    }
}
