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
        private GeneralFunctions generalFunctions;

        public TeacherController(SchoolDBContext context)
        {
            this.context = context;
            this.generalFunctions = new GeneralFunctions(context);
        }

        #region CreateTeacher
        public String CreateTeacher(string document, string name, int age)
        {
            if (!context.Teachers.Where(t => t.Document == document).Any())
            {
                Teacher teacher = new Teacher(name, document,age);
                context.Teachers.Add(teacher);
                context.SaveChanges();
                return $"The teacher {name} was created satisfactorily";
            }
            else
            {
                return $"The teacher with the document {document} already exists";
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

        #region AddSubjectToTeacher
        public String AddSubjectToTeacher(String subjectName, String teacherDocument)
        {
            var subject = context.Subjects.Include(s => s.Teachers).Where(s => s.Name == subjectName);
            var teacher = context.Teachers.Where(t => t.Document == teacherDocument);
            String checks = generalFunctions.CheckExistence(new Dictionary<string, string>() { { "teacher", teacherDocument } , { "subject", subjectName} });
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
        private void GradesByStudent(Student student, Subject subject)
        {
            Console.WriteLine($"\nStudent {student.Name}");
            Console.WriteLine($"-------------------------");
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
        }
        #endregion
    }
}
