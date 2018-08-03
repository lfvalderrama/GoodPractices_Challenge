using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    class CourseController
    {
        #region CreateCourse
        public String CreateCourse(String name, String headmanDocument, String teacherDocument)
        {
            var context = new SchoolDBContext();
            var student = context.Students.Where(s => s.Document == headmanDocument);
            var teacher = context.Teachers.Include(t => t.Course).Where(t => t.Document == teacherDocument);
            if (!context.Courses.Where(c => c.Name == name).Any())
            {
                if (teacher.Any())
                {
                    if (teacher.First().Course.Name != null)
                    {
                        return $"The teacher identified by {teacherDocument} already has assigned the course {teacher.First().Course.Name}";
                    }
                    else
                    {
                        if (student.Any())
                        {

                            Course course = new Course(name, student.First());
                            context.Courses.Add(course);
                            teacher.First().Course = course;
                            context.SaveChanges();
                            return $"The course {name} was created satisfactorily";                        
                        }
                        else
                        {
                            return $"The student with the document {headmanDocument} doesn't exists, you can't create a course without a valid headman";
                        }
                    }
                }
                else
                {
                    return $"The teacher identified by {teacherDocument} doesn't exists, you can't create a course without a teacher";
                }
            }
            else
            {
                return $"The course {name} already exists";
            }
        }
        #endregion

        #region DeleteCourse
        public String DeleteCourse(String name)
        {
            var context = new SchoolDBContext();
            var course = context.Courses.Where(c => c.Name == name);
            if (!course.Any())
            {
                return ($"The subject named {name} don't exists.");
            }
            else
            {
                try
                {
                    context.Courses.Remove(course.First());
                    context.SaveChanges();
                    return $"The course {name} was deleted satisfactorily";

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    return ($"The course can't be deleted, there are students or teachers in that Course");
                }

            }
        }
        #endregion


    }
}
