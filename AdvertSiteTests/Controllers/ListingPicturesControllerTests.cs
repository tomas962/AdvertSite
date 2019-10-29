using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class ListingPicturesControllerTests : IDisposable
    {
        private MockRepository mockRepository;

        private advert_siteContext mockadvert_siteContext;

        public ListingPicturesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            this.mockadvert_siteContext = TestHelpers.CreateFakeDbContext();

        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
            this.mockadvert_siteContext.Database.EnsureDeleted();
        }

        private ListingPicturesController CreateListingPicturesController()
        {
            return new ListingPicturesController(this.mockadvert_siteContext);
        }

        [Fact]
        public async Task GetPicture_Picture_ShouldReturnPictureFileResult()
        {
            // Arrange
            var listingPicturesController = this.CreateListingPicturesController();

            var pic = new ListingPictures() {
                PictureId = 1,
                FileName = "TEST_PICTURE.png",
                ContentType = "image/png"
            };
            mockadvert_siteContext.ListingPictures.Add(pic);
            mockadvert_siteContext.SaveChanges();

            // Act
            var result = await listingPicturesController.GetPicture(pic.PictureId);

            // Assert
            Assert.IsType<PhysicalFileResult>(result);
            var fileResult = (PhysicalFileResult)result;
            
            Assert.Equal(pic.FileName, Path.GetFileName(fileResult.FileName));
            Assert.Equal(pic.ContentType, fileResult.ContentType);
        }
    }
}
