using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class MessagesControllerTests : IDisposable
    {
        private ApplicationUser fakeUser;
        private MockRepository mockRepository;
        private advert_siteContext mockadvert_siteContext;
        private UserManager<ApplicationUser> mockUserManager;

        public MessagesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockadvert_siteContext = TestHelpers.CreateFakeDbContext();
            this.mockUserManager = TestHelpers.TestUserManager<ApplicationUser>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private MessagesController CreateMessagesController(bool withUser = false)
        {

            var contr = new MessagesController(this.mockadvert_siteContext, this.mockUserManager);
            if (withUser)
            {
                (this.fakeUser, contr.ControllerContext) = TestHelpers.FakeUserAndControllerContext(this.mockadvert_siteContext);
            }

            return contr;
        }

        [Fact]
        public void Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            // Act
            var result = messagesController.Index();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(50)]
        public async Task Inbox_ShouldReturnViewWithUserMessages(int msgCount)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            //create sender
            var user = new ApplicationUser() {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            //create messages
            for (int i = 0; i < msgCount; i++)
            {
                var msg = new Messages()
                {
                    DateSent = DateTime.Now,
                    Subject = "TestMsg"+i,
                    Text = "yo"+i
                };
                mockadvert_siteContext.Messages.Add(msg);
                mockadvert_siteContext.SaveChanges();

                var userHasMsg = new UsersHasMessages()
                {
                    MessagesId = msg.Id,
                    RecipientId = fakeUser.Id,
                    SenderId = user.Id,
                    IsDeleted = 0
                };


                mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
                mockadvert_siteContext.SaveChanges(); 
            }

            // Act
            var result = await messagesController.Inbox();
            var viewResult = (ViewResult)result;
            var messages = (IEnumerable<UsersHasMessages>)viewResult.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(msgCount, messages.Count());
        }

        [Fact]
        public void Outbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = messagesController.Outbox();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Details_ShouldGetAddedMessage()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            //create sender
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            //add msg
            var msgToAdd = new Messages()
            {
                DateSent = DateTime.Now,
                Subject = "TefstMsg",
                Text = "yoeqragh"
            };
            mockadvert_siteContext.Messages.Add(msgToAdd);
            mockadvert_siteContext.SaveChanges();

            var userHasMsg = new UsersHasMessages()
            {
                MessagesId = msgToAdd.Id,
                RecipientId = fakeUser.Id,
                SenderId = user.Id,
                IsDeleted = 0
            };
            mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
            mockadvert_siteContext.SaveChanges();

            // Act
            var result = await messagesController.Details(msgToAdd.Id);
            var viewResult = (ViewResult)result;
            var addedMsg = (Messages)viewResult.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(msgToAdd.Id, addedMsg.Id);
            Assert.Equal(msgToAdd.Text, addedMsg.Text);
            Assert.Equal(msgToAdd.Subject, addedMsg.Subject);
        }

        [Fact]
        public void CreateAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = messagesController.CreateAdmin();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = messagesController.Create();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task CreateAdmin_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            CreateMessageModel model = null;

            // Act
            var result = await messagesController.CreateAdmin(
                model);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Create_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            CreateMessageModel model = null;

            // Act
            var result = await messagesController.Create(
                model);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task MarkAsRead_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            int id = 0;
            string sender_id = null;
            string recipient_id = null;

            // Act
            var result = await messagesController.MarkAsRead(
                id,
                sender_id,
                recipient_id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            int id = 0;
            string sender_id = null;
            string recipient_id = null;

            // Act
            var result = await messagesController.Delete(
                id,
                sender_id,
                recipient_id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void UpdateUnreadMessageCount_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = messagesController.UpdateUnreadMessageCount();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetRecipientUser_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            // Act
            var result = messagesController.GetRecipientUser(this.fakeUser.Id);

            // Assert
            Assert.Equal(this.fakeUser.Id, result.Id);
        }
    }
}
