using System;
using System.Collections.Generic;
using GoodPractices_Controller;
using GoodPractices_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GoodPractices_Test
{
    [TestClass]
    public class ValidationTest
    {
        private readonly List<ForeignLanguage> dataLanguage = new List<ForeignLanguage>
        {
                new ForeignLanguage { Name = "French 1"}
        };

        private readonly List<Subject> dataSubject = new List<Subject>
        {
                new ForeignLanguage { Name = "Math 1"}
        };

        private readonly List<Student> dataStudent = new List<Student>
        {
                new Student { Document = "123456"}
        };

        private readonly List<Teacher> dataTeacher = new List<Teacher>
        {
                new Teacher { Document = "456789"}
        };

        private readonly List<Course> dataCourse = new List<Course>
        {
                new Course { Name = "5B" }
        };

        private readonly string _noExistingStudent = "45678";
        private readonly string _noExistingTeacher = "12345";
        private readonly string _noExistingCourse = "4A";
        private readonly string _noExistingSubject = "Math 2";
        private readonly string _noExistingLanguage = "French 2";

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Validation _validation;

        #region CheckExistence_return_success_for_existent_student
        [TestMethod]
        public void CheckExistence_return_success_for_existent_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "student", dataStudent[0].Document } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion
    }
}
