using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    public class TeacherController
    {
        private ISchoolDBContext _context;
        private IValidation _validator;

        public TeacherController(ISchoolDBContext context, IValidation validation)
        {
            _context = context;
            _validator = validation;
        }

        #region CreateTeacher
        public String CreateTeacher(string document, string name, int age)
        {
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "noTeacher", document } });
            if (checks != "success")
            {
                return checks;
            }
            else
            { 
                Teacher teacher = new Teacher(name, document,age);
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                return $"The teacher {name} was created satisfactorily";
            }
            
        }
        #endregion

        #region DeleteTeacher
        public String DeleteTeacher(String teacherDocument)
        {
            var teacher = _context.Teachers.Where(c => c.Document == teacherDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    _context.Teachers.Remove(teacher.First());
                    _context.SaveChanges();
                    return $"The Teacher identified with {teacherDocument} was deleted satisfactorily";

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return ($"The Teacher can't be deleted, there are subjects that have it as a teacher.");
                }

            }
        }
        #endregion

        #region GetGradesOfStudentsByTeacher
        public GradesByTeacher GetGradesOfStudentsByTeacher(string teacherDocument)
        {
            var teacher = _context.Teachers.Include(t => t.Subjects).Where(t => t.Document == teacherDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument } });
            if (checks != "success")
            {
                return new GradesByTeacher { Error = checks };
            }
            else
            {
                GradesByTeacher gradesByTeacher = new GradesByTeacher { TeacherName = teacher.First().Name, GradesBySubject = new Dictionary<string, List<GradeByStudent>>() };
                foreach (var subject in teacher.First().Subjects)
                {
                    gradesByTeacher.GradesBySubject[subject.Name] = new List<GradeByStudent>();
                    if (subject.GetType() == typeof(ForeignLanguage))
                    {
                        var students = _context.Students.Include(s => s.Grades).Where(s => s.ForeignLanguaje.Name == subject.Name);
                        foreach (var student in students)
                        {
                           gradesByTeacher.GradesBySubject[subject.Name].Add(new GradeByStudent { Name = student.Name, Grades =GradesByStudent(student, subject)});
                        }
                    }
                    else
                    {
                        var courses = _context.Courses.Include(c => c.Students).Include(c=>c.Students.Select(s=>s.Grades)).Where(c => c.Subjects.Any(s=>s.Name==subject.Name));
                        foreach (var course in courses)
                        {
                            var students = course.Students;
                            foreach (var student in students)
                            {
                                gradesByTeacher.GradesBySubject[subject.Name].Add(new GradeByStudent { Name = student.Name, Grades = GradesByStudent(student, subject) });
                            }
                        }
                    }
                }
                return gradesByTeacher;
            }
        }
        #endregion

        #region GradesByStudent
        private List<GradesBy_> GradesByStudent(Student student, Subject subject)
        {
            List<GradesBy_> gradesBy_ = new List<GradesBy_>();
            var gradesByPeriod = student.Grades.Where(g => g.Subject == subject).GroupBy(p => p.Period, (key, p) => new { Period = key, Grades = p.ToList() });
            foreach (var grades in gradesByPeriod)
            {
                var gradesOfPeriod = new GradesBy_ { Identifier = grades.Period, Grades = new List<Grade>() };
                foreach (var grade in grades.Grades)
                {
                    gradesOfPeriod.Grades.Add(grade);
                }
                gradesBy_.Add(gradesOfPeriod);
            }
            return gradesBy_;
        }
        #endregion
    }
}
