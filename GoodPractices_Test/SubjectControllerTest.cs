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
    public class SubjectControllerTest
    {

        private List<Subject> dataSubject = new List<Subject>
            {
                new Subject { Name = "Math 1", Content = "asdasdasd" }
            };

        private List<ForeignLanguage> dataLanguage = new List<ForeignLanguage>
            {
                new ForeignLanguage { Name = "English 1", Content = "asdasdasd", Language = Language.ENGLISH }
            };

        #region CreateSubject_saves_a_subject
        [TestMethod]
        public void CreateSubject_saves_a_subject()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataSubject);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Subjects).Returns(mockSet.Object);

            var controller = new SubjectController(mockContext.Object);

            //When
            controller.CreateSubject("Philosophy", "asdasd");

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Subject>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion

        #region CreateSubject_dont_saves_a_existing_subject
        [TestMethod]
        public void CreateSubject_dont_saves_a_existing_subject()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataSubject);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.Subjects).Returns(mockSet.Object);

            var controller = new SubjectController(mockContext.Object);

            //When
            controller.CreateSubject("Math 1", "asdasd");

            //then
            mockSet.Verify(m => m.Add(It.IsAny<Subject>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }
        #endregion

        #region CreateForeignLanguage_saves_a_ForeignLanguage
        [TestMethod]
        public void CreateForeignLanguage_saves_a_ForeignLanguage()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataLanguage);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.ForeignLanguages).Returns(mockSet.Object);

            var controller = new SubjectController(mockContext.Object);

            //When
            controller.CreateLanguage(Language.FRENCH,"French 1", "asdasd");

            //then
            mockSet.Verify(m => m.Add(It.IsAny<ForeignLanguage>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion

        #region CreateForeignLanguage_dont_saves_a_existing_ForeignLanguage
        [TestMethod]
        public void CreateForeignLanguage_dont_saves_a_existing_ForeignLanguaget()
        {
            //Given
            var mockSet = GeneralMock.GetQueryableMockDbSet(dataLanguage);

            var mockContext = new Mock<SchoolDBContext>();
            mockContext.Setup(c => c.ForeignLanguages).Returns(mockSet.Object);

            var controller = new SubjectController(mockContext.Object);

            //When
            controller.CreateLanguage(Language.ENGLISH, "English 1", "asdasd");

            //then
            mockSet.Verify(m => m.Add(It.IsAny<ForeignLanguage>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }
        #endregion
    }
}
