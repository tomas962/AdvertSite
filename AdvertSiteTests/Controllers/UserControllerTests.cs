using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class UserControllerTests : IDisposable
    {
        private ApplicationUser fakeUser;
        private MockRepository mockRepository;
        private advert_siteContext context;
        private UserManager<ApplicationUser> mockUserManager;

        public UserControllerTests()
        {
            this.mockUserManager = TestHelpers.TestUserManager<ApplicationUser>();
            this.context = TestHelpers.CreateFakeDbContext();
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }



        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UserController CreateUserController(bool withUser = false)
        {
            var contr = new UserController(this.context);
            if (withUser)
            {
                (this.fakeUser, contr.ControllerContext) = TestHelpers.FakeUserAndControllerContext(this.context);
            }

            return contr;
        }
        [Fact]
        public void Logout_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userController = this.CreateUserController(true);

            // Act
            var result = userController.Logout();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(false); // session
        }

        [Fact]
        public async Task DetailsShouldGetUserInformation()
        {
            var userController = CreateUserController();
            var user = new ApplicationUser()
            {
                UserName = "Mark"
            };
            var user2 = new ApplicationUser()
            {
                UserName = "Hellen"
            };
            context.Users.Add(user);
            context.Users.Add(user2);
            await context.SaveChangesAsync();

            var result = await userController.Details(user.Id);
            var viewResult = (ViewResult)result;
            var retrievedUser = (ApplicationUser)viewResult.Model;

            Assert.IsType<ViewResult>(result);
            Assert.IsType<ApplicationUser>(retrievedUser);
            Assert.Equal(retrievedUser.Id, user.Id);
        }
    }
}
