using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class ListingPicturesControllerTests : IDisposable
    {
        private MockRepository mockRepository;
        private advert_siteContext context;
        public ListingPicturesControllerTests()
        {
            this.context = TestHelpers.CreateFakeDbContext();
            this.mockRepository = new MockRepository(MockBehavior.Loose);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ListingPicturesController CreateListingPicturesController(bool withUser = false)
        {
            var listingPicController = new ListingPicturesController(
                this.context);

            return listingPicController;
        }

        [Fact]
        public async Task GetPicture_PictureId_ReturnsPictureObject()
        {
            // Arrange
            var listingPicturesController = this.CreateListingPicturesController();
            int id = 0;

            // Act
            var result = await listingPicturesController.GetPicture(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPicture_InvalidPictureId_ReturnsNotFoundView()
        {
            // Arrange
            var listingPicturesController = this.CreateListingPicturesController();

            var result = await listingPicturesController.GetPicture(453454);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
