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
        private readonly Validation _generalFunctions;

        public CourseController(SchoolDBContext context)
        {
            this.context = context;
            _generalFunctions = new Validation(context);
        }

        #region CreateCourse
        public String CreateCourse(String courseName, String headmanDocument, String teacherDocument)
        {
            var student = context.Students.Where(s => s.Document == headmanDocument);
            var teacher = context.Teachers.Include(t => t.Course).Where(t => t.Document == teacherDocument);
            String checks = _generalFunctions.CheckExistence(new Dictionary<string, string>() { { "student", headmanDocument }, { "teacher", teacherDocument }, {"noCourse", courseName} });
            if (checks != "success")
            {
                return checks;
            }
            if (teacher.First().Course != null)
            {
                return $"The teacher identified by {teacherDocument} already has assigned the course {teacher.First().Course.Name}";
            }
            else
            {
                if (!context.Courses.Where(c => c.Headman.Document == headmanDocument).Any())
                {
                    Course course = new Course(courseName, student.First());
                    course.Students = new List<Student>();
                    course.Students.Add(student.First());
                    context.Courses.Add(course);
                    teacher.First().Course = course;
                    context.SaveChanges();
                    return $"The course {courseName} was created satisfactorily";
                }
                else
                {
                    return $"The student identified by {headmanDocument} is already headman of the course {context.Courses.Where(c => c.Headman.Document == headmanDocument).First().Name}";
                }
            }
        }
        #endregion

        #region DeleteCourse
        public String DeleteCourse(String nameCourse)
        {
            var course = context.Courses.Where(c => c.Name == nameCourse);
            String checks = _generalFunctions.CheckExistence(new Dictionary<string, string>() { { "course", nameCourse } });
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
                    return ($"The course can't be deleted, there are students, subjects or teachers in that Course");
                }

            }
        }
        #endregion

        #region GetHeadmans
        public List<string> GetHeadmans()
        {
            List<string> headmanList = new List<string>();
            var courses = context.Courses.Include(c => c.Headman);
            headmanList.Add("Course........Headman's name....Headman's Document");
            foreach (var course in courses)
            {
                headmanList.Add($"{course.Name}........{course.Headman.Name}....{course.Headman.Document}");
            }
            return headmanList;
        }
        #endregion

        #region GetCourses
        public List<string> GetCourses()
        {
            List<string> coursesList = new List<string>();
            var courses = context.Courses;
            coursesList.Add("......COURSES......");
            foreach (var course in courses)
            {
                coursesList.Add($"{course.Name}");
            }
            return coursesList;
        }
        #endregion

        #region GetSubjectsByCourse
        public Dictionary<String,List<string>> GetSUbjectsByCourse()
        {
            Dictionary<String,List<string> > subjectsByCourse = new Dictionary<string, List<string>>();
            var courses = context.Courses.Include(c => c.Subjects);
            foreach (var course in courses)
            {
                subjectsByCourse[course.Name] = new List<string>();
                foreach (var subject in course.Subjects)
                {
                    subjectsByCourse[course.Name].Add($"{subject.Name}");
                }
            }
            return subjectsByCourse;
        }
        #endregion
 
    }
}
