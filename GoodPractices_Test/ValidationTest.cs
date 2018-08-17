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

        #region CheckExistence_return_error_for_existent_student
        [TestMethod]
        public void CheckExistence_return_error_for_existent_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "student", _noExistingStudent } });

            //then
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_existent_teacher
        [TestMethod]
        public void CheckExistence_return_success_for_existent_teacher()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "teacher", dataTeacher[0].Document } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_existent_teacher
        [TestMethod]
        public void CheckExistence_return_error_for_existent_teacher()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "teacher", _noExistingTeacher } });

            //then
            Assert.AreEqual($"The Teacher identified by {_noExistingTeacher} doesn't exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_existent_course
        [TestMethod]
        public void CheckExistence_return_success_for_existent_course()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "course", dataCourse[0].Name} });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_existent_course
        [TestMethod]
        public void CheckExistence_return_error_for_existent_course()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "course", _noExistingCourse } } );

            //then
            Assert.AreEqual($"The Course named {_noExistingCourse} doesn't exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_existent_subject
        [TestMethod]
        public void CheckExistence_return_success_for_existent_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "subject", dataSubject[0].Name } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_existent_subject
        [TestMethod]
        public void CheckExistence_return_error_for_existent_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "subject", _noExistingSubject } });

            //then
            Assert.AreEqual($"The Subject named {_noExistingSubject} doesn't exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_existent_ForeignLanguage
        [TestMethod]
        public void CheckExistence_return_success_for_existent_ForeignLanguage()
        {
            //Given
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "foreignLanguage", dataLanguage[0].Name } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_existent_ForeignLanguage
        [TestMethod]
        public void CheckExistence_return_error_for_existent_ForeignLanguage()
        {
            //Given
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "foreignLanguage", _noExistingLanguage } });

            //then
            Assert.AreEqual($"The ForeignLanguage named {_noExistingLanguage} doesn't exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_Noexistent_student
        [TestMethod]
        public void CheckExistence_return_success_for_Noexistent_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noStudent", _noExistingStudent } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_Noexistent_student
        [TestMethod]
        public void CheckExistence_return_error_for_Noexistent_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noStudent", dataStudent[0].Document } });

            //then
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} already exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_Noexistent_teacher
        [TestMethod]
        public void CheckExistence_return_success_for_Noexistent_teacher()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noTeacher", _noExistingTeacher } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_Noexistent_teacher
        [TestMethod]
        public void CheckExistence_return_error_for_Noexistent_teacher()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noTeacher", dataTeacher[0].Document } });

            //then
            Assert.AreEqual($"The Teacher identified by {dataTeacher[0].Document} already exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_Noexistent_course
        [TestMethod]
        public void CheckExistence_return_success_for_Noexistent_course()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noCourse", _noExistingCourse } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_Noexistent_course
        [TestMethod]
        public void CheckExistence_return_error_for_Noexistent_course()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noCourse", dataCourse[0].Name } });

            //then
            Assert.AreEqual($"The Course named {dataCourse[0].Name} already exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_Noexistent_subject
        [TestMethod]
        public void CheckExistence_return_success_for_Noexistent_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noSubject", _noExistingSubject } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_Noexistent_subject
        [TestMethod]
        public void CheckExistence_return_error_for_Noexistent_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noSubject", dataSubject[0].Name } });

            //then
            Assert.AreEqual($"The Subject named {dataSubject[0].Name} already exists", result);
        }
        #endregion

        #region CheckExistence_return_success_for_Noexistent_foreignLanguage
        [TestMethod]
        public void CheckExistence_return_success_for_Noexistent_foreignLanguage()
        {
            //Given
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noForeignLanguage", _noExistingLanguage } });

            //then
            Assert.AreEqual($"success", result);
        }
        #endregion

        #region CheckExistence_return_error_for_Noexistent_foreignLanguage
        [TestMethod]
        public void CheckExistence_return_error_for_Noexistent_foreignLanguage()
        {
            //Given
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validation = new Validation(_mockContext.Object);
            //When
            var result = _validation.CheckExistence(new Dictionary<string, string> { { "noForeignLanguage", dataLanguage[0].Name } });

            //then
            Assert.AreEqual($"The ForeignLanguage named {dataLanguage[0].Name} already exists", result);
        }
        #endregion

    }
}
