//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Data.Entity;
//using System.Linq;
//using GoodPractices_Controller;
//using GoodPractices_Model;
//using Moq;

//namespace GoodPractices_Test
//{
//    [TestClass]
//    public class TeacherControllerTest
//    {
//        private List<Teacher> dataTeacher = new List<Teacher>
//            {
//                new Teacher { Name = "BBB", Document = "12341564" }
//            };

//        #region CreateTeacher_saves_a_teacher
//        [TestMethod]
//        public void CreateTeacher_saves_a_teacher()
//        {
//            //Given            
//            var mockSet = GeneralMock.GetQueryableMockDbSet(dataTeacher);

//            var mockContext = new Mock<SchoolDBContext>();
//            mockContext.Setup(c => c.Teachers).Returns(mockSet.Object);

//            var controller = new TeacherController(mockContext.Object);

//            //When
//            controller.CreateTeacher("51684", "asdasd", 14);

//            //then
//            mockSet.Verify(m => m.Add(It.IsAny<Teacher>()), Times.Once());
//            mockContext.Verify(m => m.SaveChanges(), Times.Once());
//        }
//        #endregion

//        #region CreateTeacher_dont_saves_a_existing_teacher
//        [TestMethod]
//        public void CreateTeacher_dont_saves_a_existing_teacher()
//        {
//            //Given
//            var mockSet = GeneralMock.GetQueryableMockDbSet(dataTeacher);

//            var mockContext = new Mock<SchoolDBContext>();
//            mockContext.Setup(c => c.Teachers).Returns(mockSet.Object);

//            var controller = new TeacherController(mockContext.Object);

//            //When
//            controller.CreateTeacher("12341564", "asdasd", 14);

//            //then
//            mockSet.Verify(m => m.Add(It.IsAny<Teacher>()), Times.Never);
//            mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        }
//        #endregion
//    }
//}
