using System;
using GoodPractices_Model;
using System.Data.Entity;
using System.Linq;


namespace GoodPractices_Controller
{
    public class StudentController
    {
        private SchoolDBContext context;

        public StudentController(SchoolDBContext context)
        {
            this.context = context;
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
        public String DeleteStudent(String document)
        {
            var student = context.Students.Where(x => x.Document == document);
            if (!student.Any())
            {
                return ($"The student identified with {document} don't exists.");
            }
            else
            {
                try
                {
                    context.Students.Remove(student.First());
                    context.SaveChanges();
                    return ($"The student identified with {document} was removed satisfactorily");
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
            if (!student.Any())
            {
                return ($"The student identified with {studentDocument} don't exists.");
            }
            if (foreignLanguage.Any())
            {
                student.First().ForeignLanguaje = foreignLanguage.First();
                context.SaveChanges();
                return $"The foreign language {nameLanguage} was assigned satisfactorily to the student identified by {studentDocument}";
            }
            else
            {
                return $"The foreign language {nameLanguage} does not exists, or isn't a valid foreign language";
            }
        }
        #endregion

        #region GetGradesByPeriod
        public void GetGradesByPeriod(String studentDocument)
        {
            var student = context.Students.Include(s => s.Grades).Include(g => g.Grades.Select(s => s.Subject)).Where(s => s.Document == studentDocument);
            if (!student.Any())
            {
                Console.WriteLine($"The student identified with {studentDocument} doesn't exists.");
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
