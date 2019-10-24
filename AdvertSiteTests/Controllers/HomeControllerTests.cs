using AdvertSite.Controllers;
using AdvertSite.Models;
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
        private Mock<advert_siteContext> mockadvert_siteContext;
        private advert_siteContext dbContext;
        // Do "global" initialization here; Called before every test method. SetUp()
        public HomeControllerTests()
        {
            //this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockRepository = new MockRepository(MockBehavior.Default);

            var dbOptions = new DbContextOptionsBuilder<advert_siteContext>()
            .UseInMemoryDatabase(databaseName: "test")
            .Options;

            dbContext = new advert_siteContext(dbOptions);
            this.mockadvert_siteContext = this.mockRepository.Create<advert_siteContext>();
        }

        // Do "global" teardown here; Called after every test method. TearDown()
        public void Dispose()
        {
            this.mockRepository.VerifyAll();
            dbContext.Dispose();
        }

        private HomeController CreateHomeController()
        {
            return new HomeController(this.mockadvert_siteContext.Object);
        }

        [Fact]
        public async Task Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = new HomeController(dbContext);

            // Act
            dbContext.Add(new Category() {
                Name = "TestCat",
                Subcategory = new List<Subcategory>()
                {
                    new Subcategory()
                    {
                        Name = "TestSub"
                    }
                }
            });
            dbContext.Add(new Category()
            {
                Name = "TestCat2",
                Subcategory = new List<Subcategory>()
                {
                    new Subcategory()
                    {
                        Name = "TestSub2"
                    }
                }
            });
            dbContext.SaveChanges();

            var result = (ViewResult)await homeController.Index();
            var categories = (List<Category>)result.Model;
            
            // Assert
            Assert.True(categories.Count == 2);
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
                Assert.True(2 == count);
            }
        }

    }
}
