using GoodPractices_Model;
using GoodPractices_ResponseModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GoodPractices_Engine
{
    public class CourseEngine
    {
        private readonly ISchoolDBContext _context;
        private readonly IValidation _validator;

        public CourseEngine(ISchoolDBContext context, IValidation validation)
        {
            _context = context;
            _validator = validation;
        }

        #region CreateCourse
        public String CreateCourse(String courseName, String headmanDocument, String teacherDocument)
        {
            var student = _context.Students.Where(s => s.Document == headmanDocument);
            var teacher = _context.Teachers.Include(t => t.Course).Where(t => t.Document == teacherDocument);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "student", headmanDocument }, { "teacher", teacherDocument }, {"noCourse", courseName} });
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
                if (!_context.Courses.Where(c => c.Headman.Document == headmanDocument).Any())
                {
                    Course course = new Course(courseName, student.First())
                    {
                        Students = new List<Student>()
                    };
                    course.Students.Add(student.First());
                    _context.Courses.Add(course);
                    teacher.First().Course = course;
                    _context.SaveChanges();
                    return $"The course {courseName} was created satisfactorily";
                }
                else
                {
                    return $"The student identified by {headmanDocument} is already headman of the course {_context.Courses.Where(c => c.Headman.Document == headmanDocument).First().Name}";
                }
            }
        }
        #endregion

        #region DeleteCourse
        public String DeleteCourse(String nameCourse)
        {
            var course = _context.Courses.Where(c => c.Name == nameCourse);
            String checks = _validator.CheckExistence(new Dictionary<string, string>() { { "course", nameCourse } });
            if (checks != "success")
            {
                return checks;
            }
            else
            {
                try
                {
                    _context.Courses.Remove(course.First());
                    _context.SaveChanges();
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
            var courses = _context.Courses.Include(c => c.Headman);
            headmanList.Add("Course........Headman's name....Headman's Document");
            foreach (var course in courses)
            {
                headmanList.Add($"{course.Name}........{course.Headman.Name}....{course.Headman.Document}");
            }
            return headmanList;
        }
        #endregion

        #region GetCourses
        public /*List<string>*/ List<ResponseCourse> GetCourses()
        {
            //var coursesList = new List<Course>();
            var courses = _context.Courses.Include(c=>c.Students).Include(c=>c.Subjects);
            var coursesList = new List<ResponseCourse>();
            foreach (var c in courses)
            {
                var responseCourse = new ResponseCourse { Name = c.Name, Id = c.Id, Headman = new ResponseCourse.Info { Name = c.Headman.Name, Id = c.Headman.Id}, Students = new List<ResponseCourse.Info>() , Subjects = new List<ResponseCourse.Info>() };
                foreach (var s in c.Students)
                {
                    responseCourse.Students.Add(new ResponseCourse.Info { Name = s.Name, Id = s.Id});
                }
                foreach (var s in c.Subjects)
                {
                    responseCourse.Subjects.Add(new ResponseCourse.Info { Name = s.Name, Id = s.Id });
                }
                coursesList.Add(responseCourse);
            }
            return coursesList;
            /*coursesList.Add("......COURSES......");
            foreach (var course in courses)
            {
                coursesList.Add($"{course.Name}");
            }
            return coursesList;*/
        }
        #endregion

        #region GetSubjectsByCourse
        public Dictionary<String,List<string>> GetSUbjectsByCourse()
        {
            Dictionary<String,List<string> > subjectsByCourse = new Dictionary<string, List<string>>();
            var courses = _context.Courses.Include(c => c.Subjects);
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
