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

        private readonly List<Grade> dataGrade = new List<Grade>
        {
            new Grade {Subject = new Subject { Name = "Math 1" }, Period = "2018-1", Score = 4.2f, Type = GradeType.PARTIAL1 },
            new Grade {Subject = new Subject { Name = "Math 1" }, Period = "2018-1", Score = 4.2f, Type = GradeType.PARTIAL2 },
            new Grade {Subject = new Subject { Name = "Math 1" }, Period = "2018-1", Score = 4.2f, Type = GradeType.PARTIAL3 },
            new Grade {Subject = new Subject { Name = "Math 1" }, Period = "2018-1", Score = 4.2f, Type = GradeType.FINAL }
            };
        private readonly List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564", Grades = new List<Grade>()}
            };

        private readonly List<Subject> dataSubject = new List<Subject>
            {
                new Subject { Name = "Math 1"}
            };

        private readonly string _noExistingStudent = "123456";
        private readonly string _period = "2018-1";
        private readonly float _score = 4.2f;
        private readonly GradeType _type = GradeType.PARTIAL2;

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private GradeController _gradeController;

        #region AddPartialGradeToStudent_adds_grade
        [TestMethod]
        public void AddPartialGradeToStudent_adds_grade()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "subject", dataSubject[0].Name } })).Returns("success");
            _gradeController = new GradeController(_mockContext.Object, _validator.Object);

            //When
            var result = _gradeController.AddPartialGradeToStudent(_period, _score, dataSubject[0].Name, _type, dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The Partial grade has been added satisfactorily to the student {dataStudent[0].Name}", result);
        }
        #endregion

        #region AddPartialGradeToStudent_doesnt_add_grade_no_existing_student
        [TestMethod]
        public void AddPartialGradeToStudent_doesnt_add_grade_no_existing_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent }, { "subject", dataSubject[0].Name } })).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _gradeController = new GradeController(_mockContext.Object, _validator.Object);

            //When
            var result = _gradeController.AddPartialGradeToStudent(_period, _score, dataSubject[0].Name, _type, _noExistingStudent);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion

        #region AddPartialGradeToStudent_doesnt_add_grade_already_existing_grade
        [TestMethod]
        public void AddPartialGradeToStudent_doesnt_add_grade_already_existing_grade()
        {
            //Given
            dataGrade[0].Subject = dataSubject[0];
            dataStudent[0].Grades.Add(dataGrade[0]);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "subject", dataSubject[0].Name } })).Returns($"success");
            _gradeController = new GradeController(_mockContext.Object, _validator.Object);

            //When
            var result = _gradeController.AddPartialGradeToStudent(_period, _score, dataSubject[0].Name, GradeType.PARTIAL1, dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The subject {dataSubject[0].Name} already has a grade of that type for the period {_period}", result);
        }
        #endregion

        #region AddPartialGradeToStudent_doesnt_add_grade_FINAL
        [TestMethod]
        public void AddPartialGradeToStudent_doesnt_add_grade_FINAL()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "subject", dataSubject[0].Name } })).Returns($"success");
            _gradeController = new GradeController(_mockContext.Object, _validator.Object);

            //When
            var result = _gradeController.AddPartialGradeToStudent(_period, _score, dataSubject[0].Name, GradeType.FINAL, dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"You can only assign partial grades", result);
        }
        #endregion

        #region CalculateFinalGradeToStudent_Calculates_final_grades
        [TestMethod]
        public void CalculateFinalGradeToStudent_Calculates_final_grades()
        {
            //Given
            dataGrade[0].Subject = dataSubject[0];
            dataGrade[1].Subject = dataSubject[0];
            dataGrade[2].Subject = dataSubject[0];
            dataGrade[3].Subject = dataSubject[0];
            dataStudent[0].Grades.Add(dataGrade[0]);
            dataStudent[0].Grades.Add(dataGrade[1]);
            dataStudent[0].Grades.Add(dataGrade[2]);
            dataStudent[0].Grades.Add(dataGrade[3]);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document } })).Returns("success");
            _gradeController = new GradeController(_mockContext.Object, _validator.Object);

            //When
            var result = _gradeController.CalculateFinalGradeToStudent(_period ,dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"All final grades of the student {dataStudent[0].Name} had been calculed satisfactorily", result);
        }
        #endregion

        #region CalculateFinalGradeToStudent_doesnt_Calculates_final_grades_no_existing_student
        [TestMethod]
        public void CalculateFinalGradeToStudent_doesnt_Calculates_final_grades_no_existing_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent }})).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _gradeController = new GradeController(_mockContext.Object, _validator.Object);

            //When
            var result = _gradeController.CalculateFinalGradeToStudent(_period, _noExistingStudent);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion
    }
}
