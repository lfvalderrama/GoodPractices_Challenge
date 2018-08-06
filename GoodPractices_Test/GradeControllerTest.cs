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
    public class GradeControllerTest
    {

        private List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564", Grades = new List<Grade> { new Grade { Period = "2018-1", Subject = new Subject { Name = "Math 1" } } } }
            };

        private List<Grade> dataGrades = new List<Grade>
            {
                new Grade { Period = "2018-1", Subject = new Subject { Name = "Math 1"} }
            };

        /*
        #region AddPartialGradeToStudent_saves_a_grade
        [TestMethod]
        public void AddPartialGradeToStudent_saves_a_grade()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrades = GeneralMock.GetQueryableMockDbSet(dataGrades);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            mockContext.Setup(c => c.Grades).Returns(mockSetGrades.Object);

            var controller = new GradeController(mockContext.Object);

            //When
            controller.AddPartialGradeToStudent("2017-1", 4.2f, "English 1", GradeType.PARTIAL1, "12341564");

            //then
            mockSetGrades.Verify(m => m.Add(It.IsAny<Grade>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion
        */
    }
}
