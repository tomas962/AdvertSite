using AdvertSite.Controllers;
using AdvertSite.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class HomeControllerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<advert_siteContext> mockadvert_siteContext;

        public HomeControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockadvert_siteContext = this.mockRepository.Create<advert_siteContext>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private HomeController CreateHomeController()
        {
            return new HomeController(
                this.mockadvert_siteContext.Object);
        }

        [Fact]
        public async Task Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = await homeController.Index();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void About_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.About();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Contact_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.Contact();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Privacy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.Privacy();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Error_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = this.CreateHomeController();

            // Act
            var result = homeController.Error();

            // Assert
            Assert.True(false);
        }
    }
}
