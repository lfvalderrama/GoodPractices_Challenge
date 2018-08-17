using GoodPractices_Controller;
using GoodPractices_Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoodPractices_Test
{
    [TestClass]
    public class SubjectsControllerTest
    {             
        private readonly List<ForeignLanguage> dataLanguage = new List<ForeignLanguage>
        {
                new ForeignLanguage { Name = "French 1"}
        };

        private readonly List<Subject> dataSubject = new List<Subject>
        {
                new ForeignLanguage { Name = "Math 1"}
        };

        private readonly string _noExistingSubject = "Math 2";
        private readonly string _noExistingLanguage = "French 2";

        private Mock<ISchoolDBContext> _mockContext = new Mock<ISchoolDBContext>();
        private Mock<IValidation> _validator = new Mock<IValidation>();
        private SubjectController _subjectController;

        #region CreateSubject_saves_a_subject
        [TestMethod]
        public void CreateSubject_saves_a_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noSubject", _noExistingSubject } })).Returns("success");
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);
            //When
            var result = _subjectController.CreateSubject(_noExistingSubject, "test");

            //then
            mockSetSubject.Verify(m => m.Add(It.IsAny<Subject>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The subject {_noExistingSubject} was created satisfactorily",result);
        }
        #endregion

        #region CreateSubject_dont_save_a_existing_subject
        [TestMethod]
        public void CreateStudent_dont_save_a_existing_student()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noSubject", dataSubject[0].Name } })).Returns($"The Subject named {dataSubject[0].Name} already exists");
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);
            //When
            var result = _subjectController.CreateSubject(dataSubject[0].Name, "test");

            //then
            mockSetSubject.Verify(m => m.Add(It.IsAny<Subject>()), Times.Never());
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The Subject named { dataSubject[0].Name} already exists", result);
        }
        #endregion

        #region CreateForeignLanguage_saves_a_ForeignLanguage
        [TestMethod]
        public void CreateForeignLanguage_saves_a_ForeignLanguage()
        {
            //Given
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noForeignLanguage", _noExistingLanguage } })).Returns("success");
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);
            //When
            var result = _subjectController.CreateLanguage(Language.FRENCH,_noExistingLanguage, "test");

            //then
            mockSetLanguage.Verify(m => m.Add(It.IsAny<ForeignLanguage>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The subject {_noExistingLanguage} was created satisfactorily", result);
        }
        #endregion

        #region CreateForeignLanguage_dont_save_a_existing_ForeignLanguage
        [TestMethod]
        public void CreateForeignLanguage_dont_save_a_existing_ForeignLanguage()
        {
            //Given
            var mockSetLanguage = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguage.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "noForeignLanguage", dataLanguage[0].Name } })).Returns($"The ForeignLanguage named {dataLanguage[0].Name} already exists");
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);
            //When
            var result = _subjectController.CreateLanguage(Language.FRENCH, dataLanguage[0].Name, "test");

            //then
            mockSetLanguage.Verify(m => m.Add(It.IsAny<ForeignLanguage>()), Times.Never());
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The ForeignLanguage named {dataLanguage[0].Name} already exists",result);
        }
        #endregion

        #region DeleteSubject_deletes_a_subject
        [TestMethod]
        public void DeleteSubject_deletes_a_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "subject", dataSubject[0].Name } })).Returns("success");
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);
            //When
            var result = _subjectController.DeleteSubject(dataSubject[0].Name);

            //then
            mockSetSubject.Verify(m => m.Remove(It.IsAny<Subject>()), Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual($"The subject {dataSubject[0].Name} was deleted satisfactorily",result);
        }
        #endregion

        #region DeleteSubject_dont_delete_a_not_existing_subject
        [TestMethod]
        public void DeleteSubject_dont_delete_a_not_existing_subject()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _validator.Setup(v => v.CheckExistence(new Dictionary<string, string>() { { "subject", _noExistingSubject } })).Returns($"The Subject named {_noExistingSubject} doesn't exists");
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);
            //When
            var result = _subjectController.DeleteSubject(_noExistingSubject);

            //then
            _mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.AreEqual($"The Subject named { _noExistingSubject} doesn't exists",result);
        }
        #endregion

        #region GetSubjects_gets_subjects
        [TestMethod]
        public void GetSubjects_gets_subjects()
        {
            //Given
            var mockSetSubject = GeneralMock.GetQueryableMockDbSet(dataSubject);
            var mockSetLanguages = GeneralMock.GetQueryableMockDbSet(dataLanguage);
            _mockContext.Setup(c => c.Subjects).Returns(mockSetSubject.Object);
            _mockContext.Setup(c => c.ForeignLanguages).Returns(mockSetLanguages.Object);
            _subjectController = new SubjectController(_mockContext.Object, _validator.Object);

            //When
            var result = _subjectController.GetSubjects();

            //then
            var expected = new List<string> { "Math 1" };
            CollectionAssert.AreEqual(expected, result);
        }
        #endregion

    }
}
