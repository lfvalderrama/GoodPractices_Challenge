﻿using GoodPractices_Controller;
using GoodPractices_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoodPractices_Test
{
    [TestClass]
    public class StudentControllerTest
    {
        private readonly List<Grade> dataGrade = new List<Grade>
        {
            new Grade {Subject = new Subject { Name = "Math 1" }, Period = "2018-1", Score = 4.2f, Type = GradeType.PARTIAL1 }
        };
        private readonly List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564", Grades = new List<Grade>()}
            };

        private readonly List<ForeignLanguage> dataLanguage = new List<ForeignLanguage>
            {
                new ForeignLanguage { Name = "French 1"}
            };

        private readonly string _noExistingStudent = "123456";

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private StudentController _studentController;

        #region CreateStudent_saves_a_student
        [TestMethod]
        public void CreateStudent_saves_a_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);            
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>(){ { "noStudent", _noExistingStudent } })).Returns("success");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);
            //When
            var result = _studentController.CreateStudent(_noExistingStudent, "test", 14);

            //then
            mockSetStudent.Verify(m => m.Add(It.IsAny<Student>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual("The Student test was created satisfactorily", result);
        }
        #endregion

        #region CreateStudent_dont_saves_a_existing_student
        [TestMethod]
        public void CreateStudent_dont_saves_a_existing_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noStudent", dataStudent[0].Document } })).Returns($"The student identified by {dataStudent[0].Document} already exists");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);

            //When
            var result = _studentController.CreateStudent(dataStudent[0].Document, "asdasd", 14);

            //then
            mockSetStudent.Verify(m => m.Add(It.IsAny<Student>()), Times.Never);
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} already exists",result);
        }
        #endregion

        #region DeleteStudent_saves_change
        [TestMethod]
        public void DeleteStudent_saves_change()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document } })).Returns("success");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);

            //When
            var result = _studentController.DeleteStudent(dataStudent[0].Document);

            //then
            mockSetStudent.Verify(m => m.Remove(It.IsAny<Student>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The student identified with {dataStudent[0].Document} was removed satisfactorily", result);

        }
        #endregion

        #region DeleteStudent_dont_saves_change
        [TestMethod]
        public void DeleteStudent_dont_saves_change()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent } })).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);

            //When
            var result = _studentController.DeleteStudent(_noExistingStudent);

            //then
            mockSetStudent.Verify(m => m.Remove(It.IsAny<Student>()), Times.Never());
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists",result);
        }
        #endregion

        #region AssignForeignLanguage_do_assign_the_language
        [TestMethod]
        public void AssignForeignLanguage_do_assign_the_language()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(f => f.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "foreignLanguage", dataLanguage[0].Name } })).Returns("success");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);
            //When
            var result = _studentController.AssignForeignLanguage(dataStudent[0].Document, "French 1");

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The foreign language {dataLanguage[0].Name} was assigned satisfactorily to the student identified by {dataStudent[0].Document}",result);
        }
        #endregion

        #region AssignForeignLanguage_don't_assign_the_language
        [TestMethod]
        public void ssignForeignLanguage_dont_assign_the_language()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(f => f.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent }, { "foreignLanguage", "French 1" } })).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);
            //When
            var result = _studentController.AssignForeignLanguage(_noExistingStudent, "French 1");

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion

        #region GetGradeByPeriod_gets_grades
        [TestMethod]
        public void GetGradeByPeriod_gets_grades()
        {
            //Given
            dataStudent[0].Grades.Add(dataGrade[0]);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document } })).Returns("success");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);
            //When
            var result = _studentController.GetGradesByPeriod(dataStudent[0].Document);

            //then
            var expected = new GradeReport {
                Name = dataStudent[0].Name, Grades = new Dictionary<string, List<GradesBy_>> {
                    { "2018-1", new List<GradesBy_> {
                        new GradesBy_ {
                            Identifier = "Math 1", Grades = new List<Grade> { dataGrade[0] }
                        }
                    }
                    }
                }
            };
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Grades.Count, result.Grades.Count);
        }
        #endregion

        #region GetGradeByPeriod_gets_error
        [TestMethod]
        public void GetGradeByPeriod_gets_error()
        {
            //Given
            dataStudent[0].Grades.Add(dataGrade[0]);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent } })).Returns($"The student identified by {_noExistingStudent} doesnt exists");
            _studentController = new StudentController(_mockContext.Object, _validator.Object);
            //When
            var result = _studentController.GetGradesByPeriod(_noExistingStudent);

            //then
            Assert.AreEqual(result.Error, $"The student identified by {_noExistingStudent} doesnt exists");
        }
        #endregion

    }
}
