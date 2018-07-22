using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Journals.Model;
using Journals.Repository;
using Journals.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class SubscriberControllerTest
    {
        [TestMethod]
        public void Index_Returns_All_Journals()
        {
            Mapper.CreateMap<List<Journal>, List<SubscriptionViewModel>>();
            
            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var mockJournalRepository = Mock.Create<IJournalRepository>();
            var mockSubscriptionRepository = Mock.Create<ISubscriptionRepository>();
            Mock.Arrange(() => mockSubscriptionRepository.GetAllJournals()).IgnoreArguments().Returns(new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", FileName="TestFilename.pdf", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=1, Description="TestDesc2", FileName="TestFilename2.pdf", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            }).MustBeCalled();

            //Act
            SubscriberController controller = new SubscriberController(mockJournalRepository, mockSubscriptionRepository, membershipRepository);
            ViewResult actionResult = (ViewResult)controller.Index();
            
            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void Subscribe_Returns()
        {
            Mapper.CreateMap<List<Journal>, List<SubscriptionViewModel>>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var mockJournalRepository = Mock.Create<IJournalRepository>();
            var mockSubscriptionRepository = Mock.Create<ISubscriptionRepository>();
            Mock.Arrange(() => mockSubscriptionRepository.AddSubscription(1, 1)).IgnoreArguments().Returns(
                new OperationStatus()
                {
                   Status = true
                }).MustBeCalled();

            //Act
            SubscriberController controller = new SubscriberController(mockJournalRepository, mockSubscriptionRepository, membershipRepository);
            ActionResult actionResult = (ActionResult)controller.Subscribe(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void UnSubscribe_Returns()
        {
            Mapper.CreateMap<List<Journal>, List<SubscriptionViewModel>>();

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var mockJournalRepository = Mock.Create<IJournalRepository>();
            var mockSubscriptionRepository = Mock.Create<ISubscriptionRepository>();
            Mock.Arrange(() => mockSubscriptionRepository.UnSubscribe(1, 1)).IgnoreArguments().Returns(
                new OperationStatus()
                {
                    Status = true
                }).MustBeCalled();

            //Act
            SubscriberController controller = new SubscriberController(mockJournalRepository, mockSubscriptionRepository, membershipRepository);
            ActionResult actionResult = (ActionResult)controller.UnSubscribe(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }
    }
}
