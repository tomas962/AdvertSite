using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.EntityFrameworkCore;
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

        // Do "global" initialization here; Called before every test method. SetUp()
        public HomeControllerTests()
        {
            //this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockadvert_siteContext = this.mockRepository.Create<advert_siteContext>();
        }

        // Do "global" teardown here; Called after every test method. TearDown()
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
        public async Task example()
        {
            var options = new DbContextOptionsBuilder<advert_siteContext>()
           .UseInMemoryDatabase(databaseName: "test")
           .Options;

            using (var context = new advert_siteContext(options))
            {
                var category = new Category();
                category.Id = 1;
                category.Name = "test";

                var category2 = new Category();
                category2.Id = 2;
                category2.Name = "test";

                context.Category.Add(category);
                context.Category.Add(category2);
                context.SaveChanges();
            }

            using (var context = new advert_siteContext(options))
            {
                int count = await context.Category.CountAsync();
                Assert.True(3 == count);
            }
        }

        [Fact]
        public void About_StateUnderTest_ExpectedBehaviorAsync()
        {
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
