using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;
using GoodPractices_Controller;
using GoodPractices_Model;
using Moq;

namespace GoodPractices_Test
{
    [TestClass]
    public class CourseControllerTest
    {
        private List<Course> dataCourse = new List<Course>
            {
                new Course { Name = "BBB", Headman = new Student{Document = "1234567" } }
            };

        private List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12345" }
            };

        private List<Teacher> dataTeacher = new List<Teacher>
            {
                new Teacher { Name = "BBB", Document = "456789", Course = new Course { Name = "4A" } }
            };
        /*
        [TestMethod]
        public void CreateCourse_saves_a_course()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            

            var controller = new CourseController(mockContext.Object);

            //When
            controller.CreateCourse("4A", "12345", "456789");

            //then
            mockSetCourse.Verify(m => m.Add(It.IsAny<Course>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        /*
        [TestMethod]
        public void CreateStudent_dont_saves_a_existing_student()
        {
            //Given
            var data = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Student>>();
            mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var controller = new StudentController(mockContext.Object);

            //When
            controller.CreateStudent("12341564", "asdasd", 14);

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }*/
    }
}
