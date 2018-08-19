using GoodPractices_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    public class AdministratorController
    {
        private ISchoolDBContext _context;
        private IValidation _validator;

        public AdministratorController(ISchoolDBContext context, IValidation validation)
        {
            this._context = context;
            this._validator = validation;
        }

        #region AddStudentToCourse
        public string AddStudentToCourse(string studentDocument, string courseName)
        {
            var course = _context.Courses.Include(c => c.Students).Where(c => c.Name == courseName);
            var student = _context.Students.Where(s => s.Document == studentDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument }, { "course", courseName } });
            if (checks != "success")
            {
                return checks;
            }
            if (!_context.Courses.Where(c => c.Headman.Document == studentDocument).Any())
            {
                if (!course.First().Students.Contains(student.First()))
                {
                    if (course.First().Students.Count() < 30)
                    {
                        course.First().Students.Add(student.First());
                        _context.SaveChanges();
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
                return $"The student identified by {studentDocument} is headman in {_context.Courses.Where(c => c.Headman.Document == studentDocument).First().Name}";
            }
        }
        #endregion

        #region AddSubjectToCourse
        public String AddSubjectToCourse(String subjectName, String courseName)
        {
            {
                var course = _context.Courses.Include(c => c.Subjects).Where(c => c.Name == courseName);
                var subject = _context.Subjects.Where(s => s.Name == subjectName);
                String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "subject", subjectName }, { "course", courseName } });
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
                    _context.SaveChanges();
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
            var subject = _context.Subjects.Include(s => s.Teachers).Where(s => s.Name == subjectName);
            var teacher = _context.Teachers.Where(t => t.Document == teacherDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument }, { "subject", subjectName } });
            if (checks != "success")
            {
                return checks;
            }
            if (!subject.First().Teachers.Contains(teacher.First()))
            {
                subject.First().Teachers.Add(teacher.First());
                _context.SaveChanges();
                return $"The subject {subjectName} was assigned to the teacher identified by {teacherDocument} satisfactorily";
            }
            else
            {
                return $"The teacher identified by {teacherDocument} already has the subject {subjectName}";
            }
        }
        #endregion

        #region ReasignHeadman
        public String ReasignHeadman(String courseName, String headmanDocument)
        {
            var student = _context.Students.Where(s => s.Document == headmanDocument);
            var course = _context.Courses.Include(t => t.Students).Where(c => c.Name == courseName);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", headmanDocument }, { "course", courseName } });
            if (checks != "success")
            {
                return checks;
            }
            if (_context.Courses.Where(c => c.Headman.Document == headmanDocument).Any())
            {
                return $"The student identified by {headmanDocument} already has assigned as headman in the course {_context.Courses.Where(c => c.Headman.Document == headmanDocument).First().Name}";
            }
            else
            {
                if (course.First().Students.Contains(student.First()))
                {
                    course.First().Headman = student.First();
                    _context.SaveChanges();
                    return $"The headman of the coruse {courseName} was reasigned satisfactorily";
                }
                else
                {
                    return $"The student identified by {headmanDocument} is not in the course {courseName}";
                }
            }
        }
        #endregion
    }
}
