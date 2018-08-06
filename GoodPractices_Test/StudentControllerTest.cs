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
    public class StudentControllerTest
    {

        private List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564" }
            };

        #region CreateStudent_saves_a_student
        [TestMethod]
        public void CreateStudent_saves_a_student()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var controller = new StudentController(mockContext.Object);

            //When
            controller.CreateStudent("51684", "asdasd", 14);

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion

        #region CreateStudent_dont_saves_a_existing_student
        [TestMethod]
        public void CreateStudent_dont_saves_a_existing_student()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var controller = new StudentController(mockContext.Object);

            //When
            controller.CreateStudent("12341564", "asdasd", 14);

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }
        #endregion

        #region DeleteStudent_saves_change
        [TestMethod]
        public void DeleteStudent_saves_change()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var controller = new StudentController(mockContext.Object);

            //When
            controller.DeleteStudent("12341564");

            //then
            mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion

        #region DeleteStudent_dont_saves_change
        [TestMethod]
        public void DeleteStudent_dont_saves_change()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var controller = new StudentController(mockContext.Object);

            //When
            controller.DeleteStudent("1234156445");

            //then
            mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }
        #endregion
    }
}
