using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class UserControllerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<advert_siteContext> mockadvert_siteContext;
        private advert_siteContext context;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<advert_siteContext>()
              .UseInMemoryDatabase(databaseName: "testDb")
              .Options;

            this.context = new advert_siteContext(options);
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockadvert_siteContext = this.mockRepository.Create<advert_siteContext>(options);
        }



        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UserController CreateUserController()
        {
            return new UserController(
                this.mockadvert_siteContext.Object);
        }
        [Fact]
        public void Logout_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userController = this.CreateUserController();

            // Act
            var result = userController.Logout();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetUserState_ExpectedBehavior()
        {
            var userController = new UserController(context);
            var user = new ApplicationUser()
            {
                Id = "Testid",
                UserName = "Mark"
            };
            var user2 = new ApplicationUser()
            {
                Id = "TestID2",
                UserName = "Hellen"
            };
            context.Users.Add(user);
            context.Users.Add(user2);

            context.SaveChanges();
            var retrievedUser = userController.GetUser(user.Id);
            Assert.Equal(retrievedUser.Result.UserName, user.UserName);
        }
    }
}
