using System;
using GoodPractices_Model;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;


namespace GoodPractices_Controller
{
    public class StudentController
    {
        private ISchoolDBContext _context;
        private IValidation _validator;

        public StudentController(ISchoolDBContext context, IValidation validation)
        {
            _context = context;
            _validator = validation;
        }

        #region CreateStudent
        public String CreateStudent(string document, string name, int age)
        {
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "noStudent", document } });
            if (checks == "success")
            {
                Student student = new Student(document, name, age);
                _context.Students.Add(student);
                _context.SaveChanges();
                return $"The Student {name} was created satisfactorily";
            }
            else
            {
                return checks;
            }
        }
        #endregion

        #region DeleteStudent
        public String DeleteStudent(String studentDocument)
        {
            var student = _context.Students.Where(x => x.Document == studentDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument } });            
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    _context.Students.Remove(student.First());
                    _context.SaveChanges();
                    return ($"The student identified with {studentDocument} was removed satisfactorily");
                }                
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return ($"The student can't be deleted, there are courses with that student as headman");
                }
            }
        }
        #endregion

        #region AssignForeignLanguage
        public String AssignForeignLanguage(String studentDocument, String nameLanguage)
        {
            var student = _context.Students.Where(x => x.Document == studentDocument);
            var foreignLanguage = _context.ForeignLanguages.Where(f => f.Name == nameLanguage);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument }, {"foreignLanguage", nameLanguage } });
            if (checks != "success")
            {
                return checks;
            }
            else {
                student.First().ForeignLanguaje = foreignLanguage.First();
                _context.SaveChanges();
                return $"The foreign language {nameLanguage} was assigned satisfactorily to the student identified by {studentDocument}";
            }
        }
        #endregion

        #region GetGradesByPeriod
        public GradeReport GetGradesByPeriod(String studentDocument)
        {
            GradeReport gradeReport = new GradeReport();
            var student = _context.Students.Include(s => s.Grades).Include(g => g.Grades.Select(s => s.Subject)).Where(s => s.Document == studentDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument } });
            if (checks != "success")
            {
                gradeReport.Error = checks;
            }
            else
            {
                gradeReport.Name = student.First().Name;
                var grades = student.First().Grades.GroupBy(g => g.Period, (key, g) => new { Period = key, Grades = g.ToList() });
                Dictionary<string, List<GradesBy_>> studentGrades = new Dictionary<string, List<GradesBy_>>();
                foreach (var period in grades)
                {
                    studentGrades[period.Period] = new List<GradesBy_>();
                    var gradePerSubject = period.Grades.GroupBy(g => g.Subject, (key, g) => new { Subject = key, Grades = g.ToList() });
                    foreach (var subject in gradePerSubject)
                    {
                        GradesBy_ gradesBySubject = new GradesBy_
                        {
                            Identifier = subject.Subject.Name
                        };
                        List<Grade> scoreList = new List<Grade>();
                        foreach (var grade in subject.Grades)
                        {
                            scoreList.Add(grade);
                        }
                        gradesBySubject.Grades = scoreList;
                        studentGrades[period.Period].Add(gradesBySubject);
                    }
                }
                gradeReport.Grades = studentGrades;
            }
            return gradeReport;
        }
        #endregion
    }
}
