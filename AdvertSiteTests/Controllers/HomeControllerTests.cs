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
    public class HomeControllerTests : IDisposable
    {

        private MockRepository mockRepository;

        private advert_siteContext mockadvert_siteContext;
        private UserManager<ApplicationUser> mockUserManager;

        public HomeControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockadvert_siteContext = TestHelpers.CreateFakeDbContext();
            this.mockUserManager = TestHelpers.TestUserManager<ApplicationUser>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private HomeController CreateHomeController(bool withUser = false)
        {
            var homeController = new HomeController(this.mockadvert_siteContext);

            return homeController;
        }

        [Fact]
        public async Task Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = CreateHomeController();

            var result = await homeController.Index();

            var subCount = await mockadvert_siteContext.Subcategory.CountAsync();
            var catCount = await mockadvert_siteContext.Category.CountAsync();

            var resultView = (ViewResult)result;
            var model = (List<Category>)resultView.Model;
            int total = 0;
            foreach (var item in model)
            {
                total += item.Subcategory.Count;
            }
            Assert.IsType<ViewResult>(result);


            Assert.Equal(subCount + catCount, total + model.Count);
        }

        [Fact]
        public void About_StateUnderTest_ExpectedBehaviour()
        {
            var homeController = CreateHomeController();

            var result = homeController.About();
            var viewResult = (ViewResult)result;

            Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Message"]);
        }

        [Fact]
        public void Privacy_StateUnderTest_ExpectedBehaviour()
        {
            var homeController = CreateHomeController();

            var result = homeController.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Contact_StateUnderTest_ExpectedBehaviour()
        {
            var homeController = CreateHomeController();

            var result = homeController.Contact();
            var viewResult = (ViewResult)result;

            Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Message"]);
        }

        [Fact(Skip = "Not implemented")]
        public void Error_StateUnderTest_ExpectedBehaviour()
        {
            var homeController = CreateHomeController();

            var result = homeController.Error();
            var viewResult = (ViewResult)result;

            Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }
    }
}
