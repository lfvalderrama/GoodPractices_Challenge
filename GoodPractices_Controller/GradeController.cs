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
            var student = context.Students.Include(s => s.Grades).Where(s => s.Document == studentDocument);
            var grades = student.First().Grades.Where(g => g.Period == period).GroupBy(g => g.Subject);
            Console.WriteLine(grades.ToString());
            Dictionary<Subject, float> finalGrades = new Dictionary<Subject, float>();
            if (student.Any())
            {
                foreach (var subject in grades)
                {
                    Console.WriteLine(subject.Key);
                    finalGrades[subject.Key] = 0;
                    foreach (var grade in subject)
                    {
                        if (grade.Type == GradeType.PARTIAL1) finalGrades[subject.Key] += (float)(grade.Score * 0.3);
                        if (grade.Type == GradeType.PARTIAL2) finalGrades[subject.Key] += (float)(grade.Score * 0.3);
                        if (grade.Type == GradeType.PARTIAL3) finalGrades[subject.Key] += (float)(grade.Score * 0.4);
                    }
                    Grade new_grade = new Grade(period, finalGrades[subject.Key], subject.Key, GradeType.FINAL);
                    student.First().Grades.Add(new_grade);

                }
                context.SaveChanges();
                return $"The Partial grade has been added satisfactorily to the student {student.First().Name}";
            }
            else
            {
                return $"The student identified by {studentDocument} doesn't exists";
            }
        }
        #endregion
    }
}

