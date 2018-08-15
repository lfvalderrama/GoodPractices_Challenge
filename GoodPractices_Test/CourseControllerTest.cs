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
                new Course { Name = "3B", Headman = new Student{Document = "1234567" } }
            };

        private List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12345" }
            };

        private List<Teacher> dataTeacher = new List<Teacher>
            {
                new Teacher { Name = "BBB", Document = "456789", Course = null },
                new Teacher { Name = "BBB", Document = "456789", Course = new Course{Name = "3B" } }
            };
        
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
        
        [TestMethod]
        public void CreateCourse_dont_saves_a_Existing_course()
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
            controller.CreateCourse("3B", "12345", "456789");

            //then
            mockSetCourse.Verify(m => m.Add(It.IsAny<Course>()), Times.Never());
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
        }
    }
}
