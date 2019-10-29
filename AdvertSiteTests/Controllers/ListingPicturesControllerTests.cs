using AdvertSite.Controllers;
using AdvertSite.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class ListingPicturesControllerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<advert_siteContext> mockadvert_siteContext;

        public ListingPicturesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockadvert_siteContext = this.mockRepository.Create<advert_siteContext>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ListingPicturesController CreateListingPicturesController()
        {
            return new ListingPicturesController(
                this.mockadvert_siteContext.Object);
        }

        [Fact]
        public async Task GetPicture_StateUnderTest_ExpectedBehavior()
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
    }
}
