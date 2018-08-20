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
        private readonly List<Subject> dataSubject = new List<Subject>
        {
                new Subject { Name = "Math 1"}
        };

        private readonly List<Student> dataStudent = new List<Student>
        {
                new Student { Document = "123456"}
        };

        private readonly List<Teacher> dataTeacher = new List<Teacher>
        {
                new Teacher { Document = "456789", Course = null}
        };

        private readonly List<Course> dataCourse = new List<Course>
        {
                new Course { Name = "5B", Headman = new Student(), Subjects = new List<Subject>() }
        };

        private readonly string _noExistingStudent = "45678";
        private readonly string _noExistingTeacher = "12345";
        private readonly string _noExistingCourse = "4A";

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private CourseController _courseController;

        #region CreateCourse_creates_a_course
        [TestMethod]
        public void CreateCourse_creates_a_course()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "teacher", dataTeacher[0].Document }, {"noCourse", _noExistingCourse } })).Returns("success");
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.CreateCourse(_noExistingCourse, dataStudent[0].Document, dataTeacher[0].Document);

            //then
            mockSetCourse.Verify(c => c.Add(It.IsAny<Course>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The course {_noExistingCourse} was created satisfactorily", result);
        }
        #endregion

        #region CreateCourse_doesnt_creates_a_course_no_existing_headman
        [TestMethod]
        public void AddPartialCourseToStudent_doesnt_add_grade_no_existing_student()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", _noExistingStudent }, { "teacher", dataTeacher[0].Document }, { "noCourse", _noExistingCourse } })).Returns($"The student identified by {_noExistingStudent} doesn't exists");
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.CreateCourse(_noExistingCourse, _noExistingStudent, dataTeacher[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {_noExistingStudent} doesn't exists", result);
        }
        #endregion

        #region CreateCourse_doesnt_creates_a_course_teacher_has_already_a_course
        [TestMethod]
        public void CreateCourse_doesnt_creates_a_course_teacher_has_already_a_course()
        {
            //Given
            dataTeacher[0].Course = dataCourse[0];
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "teacher", dataTeacher[0].Document }, { "noCourse", _noExistingCourse } })).Returns($"success");
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.CreateCourse(_noExistingCourse, dataStudent[0].Document, dataTeacher[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The teacher identified by {dataTeacher[0].Document} already has assigned the course {dataCourse[0].Name}", result);
        }
        #endregion

        #region CreateCourse_doesnt_creates_a_course_headman_is_headman_of_other_course
        [TestMethod]
        public void CreateCourse_doesnt_creates_a_course_headman_is_headman_of_other_course()
        {
            //Given
            dataCourse[0].Headman = dataStudent[0];
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            var mockSetTeacher = GeneralMock.GetQueryableMockDbSet(dataTeacher);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _mockContext.Setup(c => c.Teachers).Returns(mockSetTeacher.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "student", dataStudent[0].Document }, { "teacher", dataTeacher[0].Document }, { "noCourse", _noExistingCourse } })).Returns($"success");
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.CreateCourse(_noExistingCourse, dataStudent[0].Document, dataTeacher[0].Document);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The student identified by {dataStudent[0].Document} is already headman of the course {dataCourse[0].Name}", result);
        }
        #endregion

        #region DeleteCourse_saves_change
        [TestMethod]
        public void DeleteCourse_saves_change()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "course", dataCourse[0].Name } })).Returns("success");
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.DeleteCourse(dataCourse[0].Name);

            //then
            mockSetCourse.Verify(m => m.Remove(It.IsAny<Course>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The course {dataCourse[0].Name} was deleted satisfactorily", result);

        }
        #endregion

        #region DeleteCourse_dont_saves_change
        [TestMethod]
        public void DeleteCourse_dont_saves_change()
        {
            //Given
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "course", _noExistingCourse } })).Returns($"The course named {_noExistingCourse} doesn't exists");
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.DeleteCourse(_noExistingCourse);

            //then
            mockSetCourse.Verify(m => m.Remove(It.IsAny<Course>()), Times.Never());
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The course named {_noExistingCourse} doesn't exists", result);
        }
        #endregion

        #region GetHeadmans_gets_headmans
        [TestMethod]
        public void GetHeadmans_gets_headmans()
        {
            //Given
            dataCourse[0].Headman = dataStudent[0];
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.GetHeadmans();

            //then
            var expected = new List<string> { "Course........Headman's name....Headman's Document", $"{dataCourse[0].Name}........{dataCourse[0].Headman.Name}....{dataCourse[0].Headman.Document}" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

        #region GetCourses_gets_courses
        [TestMethod]
        public void GetCourses_gets_courses()
        {
            //Given
            var mockSetStudent = GeneralMock.GetQueryableMockDbSet(dataStudent);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Students).Returns(mockSetStudent.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.GetCourses();

            //then
            var expected = new List<string> { "......COURSES......", $"{dataCourse[0].Name}" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

        #region GetSubjectsByCourse_gets_subjects
        [TestMethod]
        public void GetSubjectsByCourse_gets_subjects()
        {
            //Given
            dataCourse[0].Subjects.Add(dataSubject[0]);
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetCourse = GeneralMock.GetQueryableMockDbSet(dataCourse);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.Courses).Returns(mockSetCourse.Object);
            _courseController = new CourseController(_mockContext.Object, _validator.Object);

            //When
            var result = _courseController.GetSUbjectsByCourse();

            //then
            var expected = new Dictionary<string, List<string>> { { dataCourse[0].Name, new List<string> { dataSubject[0].Name } } };
            CollectionAssert.AreEqual(expected[dataCourse[0].Name], result[dataCourse[0].Name]);
            Assert.AreEqual(expected.Keys.First(), result.Keys.First());
        }
        #endregion
    }
}
