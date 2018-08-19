using GoodPractices_Controller;
using GoodPractices_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GoodPractices_Test
{
    [TestClass]
    public class AdministratorControllerTest
    {
        private readonly List<Course> dataCourse = new List<Course>
        {
            new Course { Name = "5A", Students = new List<Student>(), Headman = new Student{ Document = "4684"}, Subjects = new List<Subject>()},
            new Course { Name = "6A", Students = new List<Student>(), Headman = new Student{ Document = "4684"}, Subjects = new List<Subject>()}
        };
        private readonly List<Student> dataStudent = new List<Student>
            {
                new Student { Name = "BBB", Document = "12341564", Grades = new List<Grade>()}
            };

        private readonly List<Subject> dataSubject = new List<Subject>
            {
                new Subject { Name = "Math 1", Teachers = new List<Teacher>()},
                new ForeignLanguage { Name = "English 1", Teachers = new List<Teacher>()}
            };

        private readonly List<ForeignLanguage> dataLanguage = new List<ForeignLanguage>
            {
                new ForeignLanguage { Name = "English 1"}
            };

        private readonly List<Teacher> dataTeacher = new List<Teacher>
            {
                new Teacher { Name = "pablo", Document = "123456"}
            };

        private readonly string _noExistingStudent = "123456";
        private readonly string _noExistingSubject = "Math 2";

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private AdministratorController _administratorController;

        #region AddStudentTocourse_adds_student
        [TestMethod]
        public void AddStudentTocourse_adds_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns("success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddStudentToCourse(dataStudent[0].Document, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The student identified by { dataStudent[0].Document } was assigned to { dataCourse[0].Name } satisfactorily", result);
        }
        #endregion

        #region AddStudentTocourse_doesnt_adds_no_existing_student
        [TestMethod]
        public void AddStudentTocourse_doesnt_adds_no_existing_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent }, { "course", dataCourse[0].Name } })).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddStudentToCourse(_noExistingStudent, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion

        #region AddStudentTocourse_doesnt_adds_student_already_in_course
        [TestMethod]
        public void AddStudentTocourse_doesnt_adds_student_already_in_course()
        {
            //Given
            dataCourse[0].Students.Add(dataStudent[0]);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddStudentToCourse(dataStudent[0].Document, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} is already in {dataCourse[0].Name}", result);
        }
        #endregion

        #region AddStudentTocourse_doesnt_adds_student_headman_in_other_course
        [TestMethod]
        public void AddStudentTocourse_doesnt_adds_student_headman_in_other_course()
        {
            //Given
            dataCourse[1].Headman = dataStudent[0];
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddStudentToCourse(dataStudent[0].Document, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} is headman in {dataCourse[1].Name}", result);
        }
        #endregion

        #region AddStudentTocourse_doesnt_adds_student_full_course
        [TestMethod]
        public void AddStudentTocourse_doesnt_adds_student_full_course()
        {
            //Given
            for (int i = 0; i < 30; i++)
            {
                dataCourse[0].Students.Add(new Student { Document = "1354" });
            }
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddStudentToCourse(dataStudent[0].Document, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The course {dataCourse[0].Name} can't have more than 30 students", result);
        }
        #endregion

        #region AddSubjectTocourse_adds_subject
        [TestMethod]
        public void AddSubjectTocourse_adds_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "subject", dataSubject[0].Name }, { "course", dataCourse[0].Name } })).Returns("success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToCourse(dataSubject[0].Name, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The subject {dataSubject[0].Name} was assigned to {dataCourse[0].Name} satisfactorily", result);
        }
        #endregion

        #region AddSubjectTocourse_doesnt_adds_no_existing_subject
        [TestMethod]
        public void AddSubjectTocourse_doesnt_adds_no_existing_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "subject", _noExistingSubject }, { "course", dataCourse[0].Name } })).Returns($"The subject named {_noExistingSubject} doesn't exists");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToCourse(_noExistingSubject, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The subject named {_noExistingSubject} doesn't exists", result);
        }
        #endregion

        #region AddSubjectTocourse_doesnt_adds_ForeignLanguage
        [TestMethod]
        public void AddSubjectTocourse_doesnt_adds_ForeignLanguage()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "subject", dataLanguage[0].Name }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToCourse(dataLanguage[0].Name, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The subject {dataLanguage[0].Name} is a foreign language and can't be assigned to a course.", result);
        }
        #endregion

        #region AddSubjectTocourse_doesnt_adds_subject_already_in_course
        [TestMethod]
        public void AddSubjectTocourse_doesnt_adds_subject_already_in_course()
        {
            //Given
            dataCourse[0].Subjects.Add(dataSubject[0]);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "subject", dataSubject[0].Name }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToCourse(dataSubject[0].Name, dataCourse[0].Name);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The subject {dataSubject[0].Name} is already in {dataCourse[0].Name}", result);
        }
        #endregion

        #region AddSubjectToTeacher_adds_subject
        [TestMethod]
        public void AddSubjectToTeacher_adds_subject()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", dataTeacher[0].Document }, { "subject", dataSubject[0].Name } })).Returns("success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToTeacher(dataSubject[0].Name, dataTeacher[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The subject {dataSubject[0].Name} was assigned to the teacher identified by {dataTeacher[0].Document} satisfactorily", result);
        }
        #endregion

        #region AddSubjectToTeacher_doesnt_adds_no_existing_subject
        [TestMethod]
        public void AddSubjectToTeacher_doesnt_adds_no_existing_subject()
        {
            //Given
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", dataTeacher[0].Document }, { "subject", _noExistingSubject } })).Returns($"The subject named {_noExistingSubject} doesn't exists");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToTeacher(_noExistingSubject, dataTeacher[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The subject named {_noExistingSubject} doesn't exists", result);
        }
        #endregion

        #region AddSubjectToTeacher_doesnt_adds_subject_already_in_teacher
        [TestMethod]
        public void AddSubjectToTeacher_doesnt_adds_subject_already_in_teacher()
        {
            //Given
            dataSubject[0].Teachers.Add(dataTeacher[0]);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "teacher", dataTeacher[0].Document }, { "subject", dataSubject[0].Name } })).Returns("success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.AddSubjectToTeacher(dataSubject[0].Name, dataTeacher[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The teacher identified by {dataTeacher[0].Document} already has the subject {dataSubject[0].Name}", result);
        }
        #endregion

        #region ReasignHeadman_reasigns_headman
        [TestMethod]
        public void ReasignHeadman_reasigns_headman()
        {
            //Given
            dataCourse[0].Students.Add(dataStudent[0]);
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns("success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.ReasignHeadman(dataCourse[0].Name, dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The headman of the coruse {dataCourse[0].Name} was reasigned satisfactorily", result);
        }
        #endregion

        #region ReasignHeadman_doesnt_reasign_no_existing_student
        [TestMethod]
        public void ReasignHeadman_doesnt_reasign_no_existing_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent }, { "course", dataCourse[0].Name } })).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.ReasignHeadman(dataCourse[0].Name, _noExistingStudent);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion

        #region ReasignHeadman_doesnt_reasign_headman_of_other_course
        [TestMethod]
        public void ReasignHeadman_doesnt_reasign_headman_of_other_course()
        {
            //Given
            dataCourse[1].Headman = dataStudent[0] ;
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.ReasignHeadman(dataCourse[0].Name, dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} already has assigned as headman in the course {dataCourse[1].Name}", result);
        }
        #endregion

        #region ReasignHeadman_doesnt_reasign_headman_not_in_course
        [TestMethod]
        public void ReasignHeadman_doesnt_reasign_headman_not_in_course()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "course", dataCourse[0].Name } })).Returns($"success");
            _administratorController = new AdministratorController(_mockContext.Object, _validator.Object);
            //When
            var result = _administratorController.ReasignHeadman(dataCourse[0].Name, dataStudent[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} is not in the course {dataCourse[0].Name}", result);
        }
        #endregion
    }
}
