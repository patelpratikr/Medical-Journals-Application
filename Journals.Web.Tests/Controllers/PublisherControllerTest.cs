using AutoMapper;
using Journals.Model;
using Journals.Repository;
using Journals.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Journals.Web.Helpers;
using Telerik.JustMock;

namespace Journals.Web.Tests.Controllers
{
    [TestClass]
    public class PublisherControllerTest
    {
        [TestMethod]
        public void Index_Returns_All_Journals()
        {
            Mapper.CreateMap<Journal, JournalViewModel>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.GetAllJournals((int)userMock.ProviderUserKey)).Returns(new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", FileName="TestFilename.pdf", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=1, Description="TestDesc2", FileName="TestFilename2.pdf", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            ViewResult actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as IEnumerable<JournalViewModel>;

            //Assert
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void GetFile_Returns_Journals()
        {
            Mapper.CreateMap<Journal, JournalViewModel>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.GetJournalById(1)).Returns(
                    new Journal{ Id=1, Description="TestDesc", FileName="TestFilename.pdf", Title="Tester", UserId=1, ModifiedDate= DateTime.Now, Content = new byte[]{}, ContentType = "pdf"
            }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            FileContentResult actionResult = (FileContentResult)controller.GetFile(1);
            
            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void Create_Returns_Journals()
        {
            Mapper.CreateMap<Journal, JournalViewModel>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.AddJournal(new Journal()
            {
                Id = 1,
                ContentType = "pdf"
            })).IgnoreArguments().Returns(
                    new OperationStatus()
                    {
                        Status = true
                    }).MustBeCalled();

            Mapper.CreateMap<Journal, JournalViewModel>();
            Mapper.CreateMap<JournalViewModel, Journal>();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            ActionResult actionResult = controller.Create(new JournalViewModel()
            {
                ContentType = "pdf",
                Content = new byte[1],
                Id = 1,
                Description = "abc"
            });

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void Delete_Returns_Journals()
        {
            Mapper.CreateMap<Journal, JournalViewModel>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.GetJournalById(1)).Returns(
                    new Journal
                    {
                        Id = 1,
                        Description = "TestDesc",
                        FileName = "TestFilename.pdf",
                        Title = "Tester",
                        UserId = 1,
                        ModifiedDate = DateTime.Now,
                        Content = new byte[] { },
                        ContentType = "pdf"
                    }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            ActionResult actionResult = controller.Delete(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void DeletePost_Returns_Journals()
        {
            Mapper.CreateMap<JournalViewModel, Journal>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            
            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.DeleteJournal(null)).IgnoreArguments().Returns(
                new OperationStatus()
                {
                    Status   = true
                }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            ActionResult actionResult = controller.Delete(new JournalViewModel()
            {
                ContentType = "pdf",
                Content = new byte[1],
                Description = "abc",
                Id = 1
            });

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void Edit_Returns_Journals()
        {
            Mapper.CreateMap<Journal, JournalUpdateViewModel>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.GetJournalById(1)).IgnoreArguments().Returns(
                new Journal
                {
                    Id = 1,
                    Description = "TestDesc",
                    FileName = "TestFilename.pdf",
                    Title = "Tester",
                    UserId = 1,
                    ModifiedDate = DateTime.Now,
                    Content = new byte[] { },
                    ContentType = "pdf"
                }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            ActionResult actionResult = controller.Edit(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void EditPost_Returns_Journals()
        {
            Mapper.CreateMap<JournalUpdateViewModel, Journal>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.UpdateJournal(null)).IgnoreArguments().Returns(
                new OperationStatus()
                {
                    Status = true
                }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository);
            ActionResult actionResult = controller.Edit(new JournalUpdateViewModel()
            {
                ContentType = "pdf",
                Content = new byte[1],
                Description = "abc",
                Id = 1
            });

            //Assert
            Assert.IsNotNull(actionResult);
        }
    }
}