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
                    if (teacher.First().Course != null)
                    {
                        return $"The teacher identified by {teacherDocument} already has assigned the course {teacher.First().Course.Name}";
                    }
                    else
                    {
                        if (student.Any())
                        {

                            Course course = new Course(name, student.First());
                            course.Students = new List<Student>();
                            course.Students.Add(student.First());
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

        #region AddStudentToCourse
        public string AddStudentToCourse(string studentDocument, string courseName)
        {
            var context = new SchoolDBContext();
            var course = context.Courses.Include(c => c.Students).Where(c => c.Name == courseName);
            var student = context.Students.Where(s => s.Document == studentDocument);
            if (course.Any())
            {
                if (student.Any())
                {
                    if (!context.Courses.Where(c => c.Headman.Document == studentDocument).Any())
                    {
                        if (!course.First().Students.Contains(student.First()))
                        {
                            course.First().Students.Add(student.First());
                            context.SaveChanges();
                            return $"The student identified by {studentDocument} was assigned to {courseName} satisfactorily";
                        }
                        else
                        {
                            return $"The student identified by {studentDocument} is already in {courseName}";
                        }
                    }
                    else
                    {
                        return $"The student identified by {studentDocument} is headman in {context.Courses.Where(c => c.Headman.Document == studentDocument).First().Name}";
                    }
                }
                else
                {
                    return $"The student identified by {studentDocument} doesn't exists.";
                }
            }
            else
            {
                return $"The Course {courseName} doesn't exists";
            }
        }
        #endregion

        #region AddSubjectToCourse
        public String AddSubjectToCourse(String subjectName, String courseName)
        {
            {
                var context = new SchoolDBContext();
                var course = context.Courses.Include(c => c.Subjects).Where(c => c.Name == courseName);
                var subject = context.Subjects.Where(s => s.Name == subjectName);
                if (course.Any())
                {
                    if (subject.Any())
                    {
                        if (!course.First().Subjects.Contains(subject.First()))
                        {
                            course.First().Subjects.Add(subject.First());
                            context.SaveChanges();
                            return $"The subject {subjectName} was assigned to {courseName} satisfactorily";
                        }
                        else
                        {
                            return $"The subject {subjectName} is already in {courseName}";
                        }
                    }
                    else
                    {
                        return $"The subject named {subjectName} doesn't exists.";
                    }
                }
                else
                {
                    return $"The Course {courseName} doesn't exists";
                }
            }
        }
        #endregion

        #region GetHeadmans
        public void GetHeadmans()
        {
            var context = new SchoolDBContext();
            var courses = context.Courses.Include(c => c.Headman);
            Console.WriteLine("Course........Headman's name....Headman's Document");
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name}........{course.Headman.Name}....{course.Headman.Document}");
            }
        }
        #endregion

        #region GetCourses
        public void GetCourses()
        {
            var context = new SchoolDBContext();
            var courses = context.Courses;
            Console.WriteLine("......COURSES......");
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name}");
            }
        }
        #endregion

        #region GetSUbjectsByCourse
        public void GetSUbjectsByCourse()
        {
            var context = new SchoolDBContext();
            var courses = context.Courses.Include(c => c.Subjects);
            Console.WriteLine("......COURSE......");
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Name}");
                Console.WriteLine("...........SUBJECTS......");
                foreach (var subject in course.Subjects)
                {
                    Console.WriteLine($"...........{subject.Name}");
                }
            }
        }
        #endregion
    }
}
