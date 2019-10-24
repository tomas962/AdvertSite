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
        private MockRepository mockRepository;

        private Mock<advert_siteContext> mockadvert_siteContext;
        private Mock<UserManager<ApplicationUser>> mockUserManager;

        public MessagesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockadvert_siteContext = this.mockRepository.Create<advert_siteContext>();
            this.mockUserManager = this.mockRepository.Create<UserManager<ApplicationUser>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private MessagesController CreateMessagesController()
        {
            return new MessagesController(
                this.mockadvert_siteContext.Object,
                this.mockUserManager.Object);
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
        public async Task Inbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = await messagesController.Inbox();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetUserInbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            String id = null;

            // Act
            var result = messagesController.GetUserInbox(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Outbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = await messagesController.Outbox();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetUserOutbox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            String id = null;

            // Act
            var result = messagesController.GetUserOutbox(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Details_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();
            int? id = null;

            // Act
            var result = await messagesController.Details(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task CreateAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = await messagesController.CreateAdmin();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            var result = await messagesController.Create();

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
        public void getTrue_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var messagesController = this.CreateMessagesController();

            // Act
            Boolean result = messagesController.getTrue();

            // Assert
            Assert.True(result);
        }
    }
}
