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
        private SchoolDBContext context;
        private Validation generalFunctions;

        public TeacherController(SchoolDBContext context)
        {
            this.context = context;
            this.generalFunctions = new Validation(context);
        }

        #region CreateTeacher
        public String CreateTeacher(string document, string name, int age)
        {
            String checks = generalFunctions.CheckExistence(new Dictionary<string, string>() { { "noTeacher", document } });
            if (checks != "success")
            {
                return checks;
            }
            else
            { 
                Teacher teacher = new Teacher(name, document,age);
                context.Teachers.Add(teacher);
                context.SaveChanges();
                return $"The teacher {name} was created satisfactorily";
            }
            
        }
        #endregion

        #region DeleteTeacher
        public String DeleteTeacher(String teacherDocument)
        {
            var teacher = context.Teachers.Where(c => c.Document == teacherDocument);
            String checks = generalFunctions.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    context.Teachers.Remove(teacher.First());
                    context.SaveChanges();
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
        public void GetGradesOfStudentsByTeacher(string teacherDocument)
        {
            var teacher = context.Teachers.Include(t => t.Subjects).Where(t => t.Document == teacherDocument);
            String checks = generalFunctions.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument } });
            if (checks != "success")
            {
                Console.WriteLine(checks);
            }
            else
            {
                Console.WriteLine($"Grades of the students of the subjects of the teacher {teacher.First().Name}");
                foreach (var subject in teacher.First().Subjects)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Grades of the subject {subject.Name}");
                    Console.WriteLine("#####################################");
                    if (subject.GetType() == typeof(ForeignLanguage))
                    {
                        var students = context.Students.Include(s => s.Grades).Where(s => s.ForeignLanguaje.Name == subject.Name);
                        foreach (var student in students)
                        {
                            GradesByStudent(student, subject);
                        }
                    }
                    else
                    {
                        var courses = context.Courses.Include(c => c.Students).Include(c=>c.Students.Select(s=>s.Grades)).Where(c => c.Subjects.Any(s=>s.Name==subject.Name));
                        foreach (var course in courses)
                        {
                            var students = course.Students;
                            foreach (var student in students)
                            {
                                GradesByStudent(student, subject);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region GradesByStudent
        private GradeReport GradesByStudent(Student student, Subject subject)
        {
            GradeReport gradeReport = new GradeReport() { Name = student.Name };
            var gradesByPeriod = student.Grades.Where(g => g.Subject == subject).GroupBy(p => p.Period, (key, p) => new { Period = key, Grades = p.ToList() });
            foreach (var grades in gradesByPeriod)
            {
                Console.WriteLine($"Period {grades.Period}");
                Console.WriteLine("Type...........Score");
                foreach (var grade in grades.Grades)
                {
                    Console.WriteLine($"{grade.Type}..........{grade.Score}");
                }
            }
            return null;
        }
        #endregion
    }
}
