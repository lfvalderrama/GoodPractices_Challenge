using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    class TeacherController
    {
        #region CreateTeacher
        public String CreateTeacher(String name, string document, int age)
        {
            var context = new SchoolDBContext();
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
        public String DeleteTeacher(String document)
        {
            var context = new SchoolDBContext();
            var teacher = context.Teachers.Where(c => c.Document == document);
            if (!teacher.Any())
            {
                return ($"The teacehr identified with {document} doesn't exists.");
            }
            else
            {
                try
                {
                    context.Teachers.Remove(teacher.First());
                    context.SaveChanges();
                    return $"The Teacher identified with {document} was deleted satisfactorily";

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return ($"The Teacher can't be deleted, there are subjects that have it as a teacher.");
                }

            }
        }
        #endregion

        #region AddSubjectToTeacher
        public String AddSubjectToTeacher(String subjectName, String document)
        {
            var context = new SchoolDBContext();
            var subject = context.Subjects.Include(s => s.Teachers).Where(s => s.Name == subjectName);
            var teacher = context.Teachers.Where(t => t.Document == document);
            if (teacher.Any())
            {
                if (subject.Any())
                {
                    if (!subject.First().Teachers.Contains(teacher.First()))
                    {                        
                        subject.First().Teachers.Add(teacher.First());
                        context.SaveChanges();
                        return $"The subject {subjectName} was assigned to the teacher identified by {document} satisfactorily";
                    }
                    else
                    {
                        return $"The teacher identified by {document} already has the subject {subjectName}";
                    }
                }
                else
                {
                    return $"The subjects {subjectName} doesn't exists.";
                }
            }
            else
            {
                return $"The teacher identified by {document} doesn't exists";
            }
        }
        #endregion

        #region GetGradesOfStudentsByTeacher
        public String GetGradesOfStudentsByTeacher(string teacherDocument)
        {
            var context = new SchoolDBContext();
            var teacher = context.Teachers.Include(t => t.Subjects).Where(t => t.Document == teacherDocument);
            //var stud
            return "";
        }
        #endregion
    }
}
