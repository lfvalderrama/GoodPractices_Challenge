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

        private readonly List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564" }
            };

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private StudentController _studentController;

        #region CreateStudent_saves_a_student
        [TestMethod]
        public void CreateStudent_saves_a_student()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);            
            _mockContext.Setup(c => c.Students).Returns(mockSet.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>(){ { "noStudent", "51684" } })).Returns("success");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);
            //When
            var result = _studentController.CreateStudent("51684", "test", 14);

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(result, "The Student test was created satisfactorily");
        }
        #endregion

        #region CreateStudent_dont_saves_a_existing_student
        [TestMethod]
        public void CreateStudent_dont_saves_a_existing_student()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSet.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noStudent", "51684" } })).Returns("The student identified by 51684 already exists");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);

            //When
            var result = _studentController.CreateStudent("51684", "asdasd", 14);

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Never);
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(result, "The student identified by 51684 already exists");
        }
        #endregion

        #region DeleteStudent_saves_change
        [TestMethod]
        public void DeleteStudent_saves_change()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSet.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", "12341564" } })).Returns("success");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);

            //When
            var result = _studentController.DeleteStudent("12341564");

            //then
            mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(result, "The student identified with 12341564 was removed satisfactorily");

        }
        #endregion

        //#region DeleteStudent_dont_saves_change
        //[TestMethod]
        //public void DeleteStudent_dont_saves_change()
        //{
        //    //Given
        //    var mockSet = GeneralMock.GetQueryableMockDbSet(dataStudent);

        //    var mockContext = new Mock<SchoolDBContext>();
        //    mockContext.Setup(c => c.Students).Returns(mockSet.Object);

        //    var controller = new StudentController(mockContext.Object);

        //    //When
        //    controller.DeleteStudent("1234156445");

        //    //then
        //    mockSet.Verify(m => m.Remove(It.IsAny<Student>()), Times.Never);
        //    mockContext.Verify(m => m.SaveChanges(), Times.Never);
        //}
        //#endregion
    }
}
