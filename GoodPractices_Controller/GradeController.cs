using GoodPractices_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GoodPractices_Controller
{
    public class GradeController
    {
        private ISchoolDBContext _context;
        private IValidation _validator;

        public GradeController(ISchoolDBContext context, IValidation validation)
        {
            _context = context;
            _validator = validation;
        }

        #region AddPartialGradeToStudent
        public String AddPartialGradeToStudent(string period, float score, String subjectName, GradeType type, String studentDocument)
        {
            var student = _context.Students.Include(s => s.Grades).Where(s => s.Document == studentDocument);
            var subject = _context.Subjects.Where(s => s.Name == subjectName);
            var grades = student.First().Grades;
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument }, { "subject", subjectName } });
            if (checks != "success")
            {
                return checks;
            }
            if (type == GradeType.PARTIAL1 || type == GradeType.PARTIAL2 || type == GradeType.PARTIAL3)
            {               
                foreach (Grade grade in grades)
                {
                    if (grade.Period == period && grade.Subject.Id == subject.First().Id && grade.Type == type)
                    {
                        return $"The subject {subjectName} already has a grade of that type for the period {period}";
                    }
                }
                Grade new_grade = new Grade(period, score, subject.First(), type);
                student.First().Grades.Add(new_grade);
                _context.SaveChanges();
                return $"The Partial grade has been added satisfactorily to the student {student.First().Name}";                    
            }
            else
            {
                return "You can only assign partial grades";
            }
        }
        #endregion

        #region CalculateFinalGradeToStudent
        public String CalculateFinalGradeToStudent(string period, String studentDocument)
        {
            var student = _context.Students.Include(s => s.Grades).Include(g=>g.Grades.Select(s=>s.Subject)).Where(s => s.Document == studentDocument);
            Dictionary<Subject, float> finalGrades = new Dictionary<Subject, float>();
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", studentDocument } });
            if (checks != "success")
            {
                return checks;
            }
            var grades = student.First().Grades.Where(g => g.Period == period).GroupBy(g => g.Subject, (key, g) => new { Subject = key, Grades = g.ToList() });
            foreach (var subject in grades)
            {
                finalGrades[subject.Subject] = 0;
                bool hasFinal = false;
                foreach (var grade in subject.Grades)
                {
                    if (grade.Type == GradeType.PARTIAL1) finalGrades[subject.Subject] += (float)(grade.Score * 0.3);
                    if (grade.Type == GradeType.PARTIAL2) finalGrades[subject.Subject] += (float)(grade.Score * 0.3);
                    if (grade.Type == GradeType.PARTIAL3) finalGrades[subject.Subject] += (float)(grade.Score * 0.4);
                    if (grade.Type == GradeType.FINAL) hasFinal = true;
                }
                if (!hasFinal)
                {
                    Grade new_grade = new Grade(period, finalGrades[subject.Subject], subject.Subject, GradeType.FINAL);
                    student.First().Grades.Add(new_grade);
                }
                _context.SaveChanges();                    
            }
            return $"All final grades of the student {student.First().Name} had been calculed satisfactorily";
        }
        #endregion
    }
}

