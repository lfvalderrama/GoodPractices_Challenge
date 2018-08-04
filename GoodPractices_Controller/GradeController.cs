using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    class GradeController
    {
        #region AddPartialGradeToStudent
        public String AddPartialGradeToStudent(string period, float score, String subjectName, GradeType type, String studentDocument)
        {
            var context = new SchoolDBContext();
            var student = context.Students.Include(s => s.Grades).Where(s => s.Document == studentDocument);
            var subject = context.Subjects.Where(s => s.Name == subjectName);
            var grades = student.First().Grades;
            if (type == GradeType.PARTIAL1 || type == GradeType.PARTIAL2 || type == GradeType.PARTIAL3)
            {
                if (student.Any())
                {
                    if (subject.Any())
                    {
                        foreach (Grade grade in grades)
                        {
                            if (grade.Period == period && grade.Subject == subject.First() && grade.Type == type)
                            {
                                return $"The subject {subjectName} already has a grade of that type for the period {period}";
                            }
                        }
                        Grade new_grade = new Grade(period, score, subject.First(), type);
                        student.First().Grades.Add(new_grade);
                        context.SaveChanges();
                        return $"The Partial grade has been added satisfactorily to the student {student.First().Name}";
                    }
                    else
                    {
                        return $"The subject {subjectName} doesn't exists";
                    }
                }
                else
                {
                    return $"The student identified by {studentDocument} doesn't exists";
                }
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
            var context = new SchoolDBContext();
            var student = context.Students.Include(s => s.Grades).Include(g=>g.Grades.Select(s=>s.Subject)).Where(s => s.Document == studentDocument);
            Dictionary<Subject, float> finalGrades = new Dictionary<Subject, float>();
            if (student.Any())
            {
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
                    context.SaveChanges();                    
                }
                return $"All final grades of the student {student.First().Name} had been calculed satisfactorily";
            }
            else
            {
                return $"The student identified by {studentDocument} doesn't exists";
            }
        }
        #endregion
    }
}

