using GoodPractices_Engine;
using GoodPractices_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoodPractices_Test
{
    [TestClass]
    public class TeacherControllerTest
    {
        private readonly List<Grade> dataGrade = new List<Grade>
        {
            new Grade {Subject = new Subject { Name = "Math 1" }, Period = "2018-1", Score = 4.2f, Type = GradeType.PARTIAL1 }
        };
        private readonly List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564", Grades = new List<Grade>()}
            };

        private readonly List<Teacher> dataTeacher = new List<Teacher>
            {
                new Teacher {Document = "456789", Name = "Pedro", Subjects = new List<Subject>() }
            };

        private readonly List<Subject> dataSubject = new List<Subject>
            {
                new Subject {Name = "Math 1"}
            };

        private readonly List<Course> dataCourse = new List<Course>
        {
            new Course { Name = "5A", Students= new List<Student>(), Subjects = new List<Subject>() }
        };
        private readonly string _noExistingTeacher = "135131";

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private TeacherEngine _teacherController;

        #region CreateSTeacher_saves_a_teacher
        [TestMethod]
        public void CreateSTeacher_saves_a_teacher()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noTeacher", _noExistingTeacher } })).Returns("success");
            _teacherController = new TeacherEngine(_mockContext.Object, _validator.Object);
            //When
            var result = _teacherController.CreateTeacher(_noExistingTeacher, "test", 14);

            //then
            mockSetTeacher.Verify(m => m.Add(It.IsAny<Teacher>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual("The teacher test was created satisfactorily", result);
        }
        #endregion

        #region CreateTeacher_dont_saves_a_existing_Teacher
        [TestMethod]
        public void CreateTeacher_dont_saves_a_existing_Teacher()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noTeacher", dataTeacher[0].Document } })).Returns($"The Teacher identified by {dataTeacher[0].Document} already exists");
            _teacherController = new TeacherEngine(_mockContext.Object, _validator.Object);

            //When
            var result = _teacherController.CreateTeacher(dataTeacher[0].Document, "asdasd", 14);

            //then
            mockSetTeacher.Verify(m => m.Add(It.IsAny<Teacher>()), Times.Never);
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual($"The Teacher identified by {dataTeacher[0].Document} already exists", result);
        }
        #endregion

        #region DeleteTeacher_saves_change
        [TestMethod]
        public void DeleteTeacher_saves_change()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", dataTeacher[0].Document } })).Returns("success");
            _teacherController = new TeacherEngine(_mockContext.Object, _validator.Object);

            //When
            var result = _teacherController.DeleteTeacher(dataTeacher[0].Document);

            //then
            mockSetTeacher.Verify(m => m.Remove(It.IsAny<Teacher>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The Teacher identified with {dataTeacher[0].Document} was deleted satisfactorily", result);

        }
        #endregion

        #region DeleteTeacher_dont_saves_change
        [TestMethod]
        public void DeleteTeacher_dont_saves_change()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", _noExistingTeacher } })).Returns($"The Teacher identified by {_noExistingTeacher} doesn't exists");
            _teacherController = new TeacherEngine(_mockContext.Object, _validator.Object);

            //When
            var result = _teacherController.DeleteTeacher(_noExistingTeacher);

            //then
            mockSetTeacher.Verify(m => m.Remove(It.IsAny<Teacher>()), Times.Never());
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The Teacher identified by {_noExistingTeacher} doesn't exists", result);
        }
        #endregion

        #region GetGradesOfStudentsByTeacher_gets_grades
        [TestMethod]
        public void GetGradesOfStudentsByTeacher_gets_grades()
        {
            //Given
            dataTeacher[0].Subjects.Add(dataSubject[0]);
            dataStudent[0].Grades.Add(dataGrade[0]);
            dataCourse[0].Students.Add(dataStudent[0]);
            dataCourse[0].Subjects.Add(dataSubject[0]);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", dataTeacher[0].Document } })).Returns("success");
            _teacherController = new TeacherEngine(_mockContext.Object, _validator.Object);

            //When
            var result = _teacherController.GetGradesOfStudentsByTeacher(dataTeacher[0].Document);

            //then
            var expected = new GradesByTeacher
            {
                TeacherName = dataTeacher[0].Name,
                GradesBySubject = new Dictionary<string, List<GradeByStudent>>
                {
                    { dataTeacher[0].Subjects[0].Name , new List<GradeByStudent>
                        {
                            new GradeByStudent
                            {
                                Name = dataStudent[0].Name , Grades = new List<GradesBy_>
                                {
                                    new GradesBy_
                                    {
                                        Identifier = dataGrade[0].Period, Grades = new List<Grade>
                                        {
                                            dataGrade[0]
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            Assert.AreEqual(expected.TeacherName, result.TeacherName);
            Assert.AreEqual(expected.GradesBySubject.Count, result.GradesBySubject.Count);
        }
        #endregion

        #region GetGradesOfStudentsByTeacher_gets_error
        [TestMethod]
        public void GetGradesOfStudentsByTeacher_gets_error()
        {
            //Given
            dataTeacher[0].Subjects.Add(dataSubject[0]);
            dataStudent[0].Grades.Add(dataGrade[0]);
            dataCourse[0].Students.Add(dataStudent[0]);
            dataCourse[0].Subjects.Add(dataSubject[0]);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetGrade = GeneralMock.GetQueryableMockDbSet(dataGrade);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.Grades).Returns(mockSetGrade.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", _noExistingTeacher } })).Returns($"The Teacher identified by { _noExistingTeacher} doesn't exists");
            _teacherController = new TeacherEngine(_mockContext.Object, _validator.Object);

            //When
            var result = _teacherController.GetGradesOfStudentsByTeacher(_noExistingTeacher);

            //then            
            Assert.AreEqual($"The Teacher identified by {_noExistingTeacher} doesn't exists", result.Error);
        }
        #endregion
    }
}
