using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Http;
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
                    Subject = "TestMsg" + i,
                    Text = "yo" + i
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

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public async Task Outbox_StateUnderTest_ExpectedBehavior(int msgCount)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            //recipient sender
            var user = new ApplicationUser()
            {
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
                    Subject = "TestMsg" + i,
                    Text = "yo" + i
                };
                mockadvert_siteContext.Messages.Add(msg);
                mockadvert_siteContext.SaveChanges();

                var userHasMsg = new UsersHasMessages()
                {
                    MessagesId = msg.Id,
                    RecipientId = user.Id,
                    SenderId = fakeUser.Id,
                    IsDeleted = 0,
                    IsAdminMessage = 0
                };

                mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
                mockadvert_siteContext.SaveChanges();
            }

            // Act
            var result = await messagesController.Outbox();
            var viewResult = (ViewResult)result;
            var messages = (IEnumerable<UsersHasMessages>)viewResult.Model;
            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(msgCount, messages.Count());
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
            var messagesController = this.CreateMessagesController(true);
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Query["recipientId"]).Returns(this.fakeUser.Id);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };
            messagesController.ControllerContext = controllerContext;
            // Act


            var result = messagesController.CreateAdmin();
            var resultView = (ViewResult)result;
            var sender = (CreateMessageModel)resultView.Model;


            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<CreateMessageModel>(resultView.Model);
            Assert.Equal(this.fakeUser.Id, sender.RecipientId);
        }

        [Fact(Skip = "Something wrong at .create()")]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            var messagesController = this.CreateMessagesController(true);
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Query["recipientId"]).Returns(user.Id);
            httpContext.Setup(x => x.Request.Query["subject"]).Returns(this.fakeUser.Id);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };
            messagesController.ControllerContext = controllerContext;


            // Act


            var result = messagesController.Create();
            var resultView = (ViewResult)result;
            var sender = (CreateMessageModel)resultView.Model;


            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<CreateMessageModel>(resultView.Model);
            Assert.Equal(this.fakeUser.Id, sender.RecipientId);
        }

        [Fact]
        public async Task CreateAdmin_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            }; 
            var user2 = new ApplicationUser()
            {
                UserName = "Ben",
                Email = "Smithinges@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user2);
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();
            CreateMessageModel model = new CreateMessageModel()
            {
                RecipientId = this.fakeUser.Id,
                Message = new Messages()
                {
                    Subject = "test",
                    Text = "This is a test message"
                }
                
            };
            // Act
            var result = await messagesController.CreateAdmin(model);
            var messageCount = mockadvert_siteContext.UsersHasMessages.Where(x => x.IsAdminMessage == 1 && x.SenderId.Equals(this.fakeUser.Id)).Count();
            var userCount = mockadvert_siteContext.Users.Count();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(userCount, messageCount);
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
            var messagesController = this.CreateMessagesController(true);
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);

            var msg = new UsersHasMessages()
            {
                Messages = new Messages()
                {
                    Subject = "Test Subject",
                    Text = "Test Text"
                },
                RecipientId = this.fakeUser.Id,
                SenderId = user.Id,
                IsAdminMessage = 0,
                AlreadyRead = 0,
                IsDeleted = 0
            };
            mockadvert_siteContext.UsersHasMessages.Add(msg);

            mockadvert_siteContext.SaveChanges();

            // Act
            var result = await messagesController.MarkAsRead(msg.MessagesId, user.Id, fakeUser.Id);

            var userMessage = mockadvert_siteContext.UsersHasMessages.FirstOrDefault(x => x.MessagesId == msg.MessagesId && user.Id.Equals(x.SenderId) && fakeUser.Id.Equals(x.RecipientId));

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(userMessage.AlreadyRead == 1);
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

        [Theory]
        [InlineData(2, 2)]
        [InlineData(10, 8)]
        [InlineData(10, 2)]
        public void UpdateUnreadMessageCount_StateUnderTest_ExpectedBehavior(int messageCount, int unreadCount)
        {
            // Arrange
            var messagesController = this.CreateMessagesController(true);
            //recipient sender
            var user = new ApplicationUser()
            {
                UserName = "John",
                Email = "aerth@gmail.com"
            };
            mockadvert_siteContext.Users.Add(user);
            mockadvert_siteContext.SaveChanges();

            int counter = 0;
            //create messages
            for (int i = 0; i < messageCount; i++)
            {
                var msg = new Messages()
                {
                    DateSent = DateTime.Now,
                    Subject = "TestMsg" + i,
                    Text = "yo" + i
                };
                mockadvert_siteContext.Messages.Add(msg);
                mockadvert_siteContext.SaveChanges();

                var userHasMsg = new UsersHasMessages()
                {
                    MessagesId = msg.Id,
                    RecipientId = fakeUser.Id,
                    SenderId = user.Id,
                    IsDeleted = 0,
                    IsAdminMessage = 0,
                    AlreadyRead = 1
                };

                if (counter < unreadCount)
                {
                    userHasMsg.AlreadyRead = 0;
                    counter++;
                }

                mockadvert_siteContext.UsersHasMessages.Add(userHasMsg);
                mockadvert_siteContext.SaveChanges();
            }

            // Act
            var result = messagesController.UpdateUnreadMessageCount();

            // Assert
            Assert.Equal(unreadCount, result);
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
