using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodPractices_Model;
using System.Data.Entity;

namespace GoodPractices_Controller
{
    public class CourseController
    {
        private SchoolDBContext context;
        private GeneralFunctions generalFunctions;

        public CourseController(SchoolDBContext context)
        {
            this.context = context;
            this.generalFunctions = new GeneralFunctions(context);
        }
        #region CreateCourse
        public String CreateCourse(String name, String headmanDocument, String teacherDocument)
        {
            var student = context.Students.Where(s => s.Document == headmanDocument);
            var teacher = context.Teachers.Include(t => t.Course).Where(t => t.Document == teacherDocument);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "student", headmanDocument }, { "teacher", teacherDocument } });
            if (checks != "success")
            {
                return checks;
            }
            if (!context.Courses.Where(c => c.Name == name).Any())
            {
                if (teacher.First().Course != null)
                {
                    return $"The teacher identified by {teacherDocument} already has assigned the course {teacher.First().Course.Name}";
                }
                else
                {
                    if (!context.Courses.Where(c => c.Headman.Document == headmanDocument).Any())
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
                        return $"The student identified by {headmanDocument} is already headman of the course {context.Courses.Where(c => c.Headman.Document == headmanDocument).First().Name}";
                    }
                }
            }
            else
            {
                return $"The course {name} already exists";
            }
        }
        #endregion

        #region DeleteCourse
        public String DeleteCourse(String nameCourse)
        {
            var course = context.Courses.Where(c => c.Name == nameCourse);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "course", nameCourse } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    context.Courses.Remove(course.First());
                    context.SaveChanges();
                    return $"The course {nameCourse} was deleted satisfactorily";

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
            var course = context.Courses.Include(c => c.Students).Where(c => c.Name == courseName);
            var student = context.Students.Where(s => s.Document == studentDocument);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "student", studentDocument }, { "course", courseName } });
            if (checks != "success")
            {
                return checks;
            }
            if (!context.Courses.Where(c => c.Headman.Document == studentDocument).Any())
            {
                if (!course.First().Students.Contains(student.First()))
                {
                    if (course.First().Students.Count() < 30)
                    {
                        course.First().Students.Add(student.First());
                        context.SaveChanges();
                        return $"The student identified by {studentDocument} was assigned to {courseName} satisfactorily";
                    }
                    else
                    {
                        return $"The course {courseName} can't have more than 30 students";
                    }
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
        #endregion

        #region AddSubjectToCourse
        public String AddSubjectToCourse(String subjectName, String courseName)
        {
            {
                var course = context.Courses.Include(c => c.Subjects).Where(c => c.Name == courseName);
                var subject = context.Subjects.Where(s => s.Name == subjectName);
                String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "subject", subjectName }, { "course", courseName } });
                if (checks != "success")
                {
                    return checks;
                }
                if(subject.First().GetType() == typeof(ForeignLanguage))
                {
                    return $"The subject {subjectName} is a foreign language and can't be assigned to a course.";
                }
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
        }
        #endregion

        #region GetHeadmans
        public void GetHeadmans()
        {
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
            var courses = context.Courses.Include(c => c.Subjects);
            foreach (var course in courses)            {

                Console.WriteLine($"\n\n..........COURSE {course.Name} .......");
                Console.WriteLine("\n...........SUBJECTS.......");
                foreach (var subject in course.Subjects)
                {
                    Console.WriteLine($"{subject.Name}");
                }
            }
        }
        #endregion
                
        #region ReasignHeadman
        public String ReasignHeadman(String courseName, String headmanDocument)
        {
            var student = context.Students.Where(s => s.Document == headmanDocument);
            var course = context.Courses.Include(t => t.Students).Where(c => c.Name == courseName);
            String checks = generalFunctions.checkExistence(new Dictionary<string, string>() { { "student", headmanDocument }, { "course", courseName } });
            if (checks != "success")
            {
                return checks;
            }
            if (context.Courses.Where(c => c.Headman.Document == headmanDocument).Any())
            {
                return $"The student identified by {headmanDocument} already has assigned as headman in the course {context.Courses.Where(c => c.Headman.Document == headmanDocument).First().Name}";
            }
            else
            {
                if (course.First().Students.Contains(student.First()))
                {
                    course.First().Headman = student.First();
                    context.SaveChanges();
                    return $"The headman of the coruse {courseName} was reasigned satisfactorily";
                }
                else
                {
                    return $"The student identified by {headmanDocument} is not in the course {courseName}";
                }
            }                
        }
        #endregion
    }
}
