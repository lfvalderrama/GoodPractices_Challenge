using System;
using GoodPractices_Model;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;


namespace GoodPractices_Controller
{
    public class StudentController
    {
        private SchoolDBContext context;
        private GeneralFunctions generalFunctions;

        public StudentController(SchoolDBContext context)
        {
            this.context = context;
            this.generalFunctions = new GeneralFunctions(context);
        }

        #region CreateStudent
        public String CreateStudent(string document, string name, int age)
        {          
            if (!context.Students.Where(x => x.Document == document).Any())
            {
                Student student = new Student(document, name, age);
                context.Students.Add(student);
                context.SaveChanges();
                return $"The Student {name} was created satisfactorily";
            }
            else
            {
                return ($"The student identified with {document} already exists.");
            }
        }
        #endregion

        #region DeleteStudent
        public String DeleteStudent(String studentDocument)
        {
            var student = context.Students.Where(x => x.Document == studentDocument);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "student", studentDocument } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    context.Students.Remove(student.First());
                    context.SaveChanges();
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
            var student = context.Students.Where(x => x.Document == studentDocument);
            var foreignLanguage = context.ForeignLanguages.Where(f => f.Name == nameLanguage);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "student", studentDocument }, {"foreignLanguage", nameLanguage } });
            if (checks != "success")
            {
                return checks;
            }
            else {
                student.First().ForeignLanguaje = foreignLanguage.First();
                context.SaveChanges();
                return $"The foreign language {nameLanguage} was assigned satisfactorily to the student identified by {studentDocument}";
            }
        }
        #endregion

        #region GetGradesByPeriod
        public void GetGradesByPeriod(String studentDocument)
        {
            var student = context.Students.Include(s => s.Grades).Include(g => g.Grades.Select(s => s.Subject)).Where(s => s.Document == studentDocument);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "student", studentDocument } });
            if (checks != "success")
            {
                Console.WriteLine(checks);
            }
            else
            {
                Console.WriteLine($"Grades of the student {student.First().Name}");
                var grades = student.First().Grades.GroupBy(g => g.Period, (key, g) => new { Period = key, Grades = g.ToList() });
                foreach (var period in grades)
                {
                    Console.WriteLine($"\nPeriod {period.Period}");
                    Console.WriteLine(".......................");
                    var gradePerSubject = period.Grades.GroupBy(g => g.Subject, (key, g) => new { Subject = key, Grades = g.ToList() });
                    foreach (var subject in gradePerSubject)
                    {
                        Console.WriteLine($"\nSubject {subject.Subject.Name}");
                        Console.WriteLine("........................");
                        Console.WriteLine($"Type...........Score");
                        foreach (var grade in subject.Grades)
                        {
                            Console.WriteLine($"{grade.Type}.........{grade.Score}");
                        }
                    }
                }
            }
        }
        #endregion
    }
}
