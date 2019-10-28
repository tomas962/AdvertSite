using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
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

        private MessagesController CreateMessagesController(bool withUser)
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
            var messagesController = this.CreateMessagesController();

            // Act
            var result = messagesController.Index();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Inbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);

            // Act
            var result = messagesController.Inbox();

            // Assert
            Assert.True(false);
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
        public void Details_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            int? id = null;

            // Act
            var result = messagesController.Details(
                id);

            // Assert
            Assert.True(false);
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
        public void GetUserInbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            String userId = null;

            // Act
            var result = messagesController.GetUserInbox(
                userId);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetUserOutbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            String userId = null;

            // Act
            var result = messagesController.GetUserOutbox(
                userId);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetMessage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            int id = 0;

            // Act
            var result = messagesController.GetMessage(
                id);

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
            var messagesController = this.CreateMessagesController();
            String id = null;

            // Act
            var result = messagesController.GetRecipientUser(
                id);

            // Assert
            Assert.True(false);
        }
    }
}
