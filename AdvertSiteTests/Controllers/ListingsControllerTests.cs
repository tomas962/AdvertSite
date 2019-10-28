using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class ListingsControllerTests : IDisposable
    {
        private ApplicationUser fakeUser;
        private MockRepository mockRepository;
        private advert_siteContext context;
        private UserManager<ApplicationUser> mockUserManager;

        public ListingsControllerTests()
        {
            this.mockUserManager = TestHelpers.TestUserManager<ApplicationUser>();
            this.context = TestHelpers.CreateFakeDbContext();
            this.mockRepository = new MockRepository(MockBehavior.Strict);


            List<Category> categoryList = new List<Category>()
            {
                new Category()
                {
                    Name = "Komp techn.",
                    Id = 1
                },
                new Category()
                {
                    Name = "buitis",
                    Id = 2
                }
            };
            List<Subcategory> subcategoryList = new List<Subcategory>()
            {
                new Subcategory()
                {
                    Name = "Klaviaturos",
                    Id = 1,
                    Categoryid = 1
                },
                new Subcategory()
                {
                    Name = "Peles",
                    Id = 2,
                    Categoryid = 1
                },
                new Subcategory()
                {
                    Name = "Stalas",
                    Id = 3,
                    Categoryid = 2
                },
                new Subcategory()
                {
                    Name = "Kedes",
                    Id = 4,
                    Categoryid = 2
                }
            };

        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ListingsController CreateListingsController(Boolean withUser = false)
        {
            var listingController =  new ListingsController(
                this.context, this.mockUserManager);
            if (withUser)
            {
                (fakeUser, listingController.ControllerContext) = TestHelpers.FakeUserAndControllerContext(context);
            }

            return listingController;

        }

        [Fact]
        public async Task CreateListing_StateUnderTest_ExpectedBehavior()
        {
            var listingsController = this.CreateListingsController(true);

            ListingNewModel newListing1 = new ListingNewModel()
            {
                Name = "TESTNAME",
                Description = "DESCT",
                Subcategoryid = 1,
                Price = 500.0,
                GoogleLatitude = 0,
                GoogleLongitude = 0,
                GoogleRadius = 0
            };
            ListingNewModel newListing2 = new ListingNewModel()
            {
                Name = "test2",
                Description = "test2",
                Subcategoryid = 50,
                Price = 500.0,
                GoogleLatitude = 0,
                GoogleLongitude = 0,
                GoogleRadius = 0
            };

            listingsController.CreateListing(newListing1);
            listingsController.CreateListing(newListing2);

            var listings = await context.Listings.ToListAsync();
            Assert.True(listings.Count == 2);
        }

        [Fact]
        public async Task ApproveUserListing_StateUnderTest_ExpectedBehavior()
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var listings = await context.Listings.ToListAsync();

           await  listingsController.ApproveListing(listings[0].Id);

            Assert.True(listings[0].Verified == 1);
        }

        [Fact]
        public async Task DeleteUserListing_StateUnderTest_ExpectedBehavior()
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing()
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var listings = await context.Listings.ToListAsync();

            await listingsController.DeleteUserListing(listings[0].Id);
            await listingsController.DeleteUserListing(listings[1].Id);
            var listing1 = context.Listings.Find(listings[0].Id);
            var listing2 = context.Listings.Find(listings[1].Id);

            var listingsAfterRemoval = await context.Listings.ToListAsync();

            Assert.Null(listing1);
            Assert.Null(listing2);
        }

        [Fact]
        public async Task HideUserListing_StateUnderTest_ExpectedBehavior()
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(display: 1),
                GenerateListing(display: 1),
                GenerateListing(display: 1),
                GenerateListing(display: 1),
                GenerateListing(display: 1),
                GenerateListing(display: 1),
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var listings = await context.Listings.ToListAsync();
            await listingsController.HideUserListing(listings[0].Id);

            var listing = await context.Listings.FirstOrDefaultAsync(x => x.Id == listings[0].Id);
            Assert.True(listing.Display == 0);
        }

        [Fact]
        public async Task EditUserListing_StateUnderTest_ExpectedBehavior()
        {
            int id = 150000;
            var listingsController = this.CreateListingsController();

            var listing = GenerateListing(name: "TESTNAME", description: "TESTDESC", display: 0, verified: 1);
            listing.Id = id;

            context.Listings.Add(listing);
            await context.SaveChangesAsync();

            listing.Name = "NEWNAME";
            listing.Description = "NEWDESC";

            await listingsController.EditUserListing(listing);

            var newListingResult = context.Listings.Find(id);

            Assert.Equal(listing.Name, newListingResult.Name);
            Assert.Equal(listing.Description, newListingResult.Description);
            Assert.True(newListingResult.Display == 1);
            Assert.True(newListingResult.Verified == 0);
        }

        [Theory]
        [InlineData("Search", "mouse", null, 4)]
        [InlineData("Category", null, 1, 9)]
        [InlineData("Subcategory", null, 1, 7)]
        [InlineData("!@#!@$!@$", "ASDFAEWQWE", 4564654, 13)]
        public async Task GetListingsByCategories_StateUnderTest_ExpectedBehavior(String type, String key, int id, int expectedResult)
        {
            foreach (var entity in context.Listings)
                context.Listings.Remove(entity);
            await context.SaveChangesAsync();
            // Arrange
            
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(subcategoryid: 1, name: "mouses", verified: 1, display: 1),
                GenerateListing(subcategoryid: 1, name: "mous456457568666es", verified: 1, display: 1),
                GenerateListing(subcategoryid: 1, name: "mous12eafsdgdbfdfes", verified: 1, display: 1),
                GenerateListing(subcategoryid: 4, name: "1234", verified: 1, display: 1),
                GenerateListing(subcategoryid: 1, name: "mou553453456ses", verified: 1, display: 1),
                GenerateListing(subcategoryid: 1, name: "MoUSe", verified: 1, display: 1),
                GenerateListing(subcategoryid: 1, name: "MOOOSE", verified: 1, display: 1),
                GenerateListing(subcategoryid: 1, name: "MOUSE", verified: 1, display: 1),
                GenerateListing(subcategoryid: 2, name: "mouse", verified: 1, display: 1),
                GenerateListing(subcategoryid: 2, name: "random", verified: 1, display: 1),
                GenerateListing(subcategoryid: 3, name: "random", verified: 1, display: 1),
                GenerateListing(subcategoryid: 3, name: "random", verified: 1, display: 1),
                GenerateListing(subcategoryid: 4, name: "keyboard", verified: 1, display: 1),
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();
            var listingList = await listingsController.GetListingsByCategoriesAndSubCategories(queryType: type, queryKey:key, id: id);
           
            Assert.True(listingList.Count == expectedResult);
        }

        [Fact(Skip = ".Include() throws null pointer exception")]
        public async Task GetListingWithAdditionalInformation_StateUnderTest_ExpectedBehavior()
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
            };

            list[0].Id = 500;
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();
            var test = await context.Listings.ToListAsync();

            var result = await listingsController.GetListingWithoutAdditionalsInfomration(500);

            Assert.True(result.Id == 500);
        }


        [Theory]
        [InlineData(6783)]
        [InlineData(4231)]
        public async Task GetListingWithoutAdditionalsInfomration_StateUnderTest_ExpectedBehavior(int id)
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
            };

            list[0].Id = id;
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();
            var test = await context.Listings.ToListAsync();

            var result = await listingsController.GetListingWithoutAdditionalsInfomration(id);

            Assert.True(result.Id == id);
        }

        [Fact]
        public async Task GetUncomfirmedListings_StateUnderTest_ExpectedBehavior()
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
            };
            
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.GetUncomfirmedListings();


            Assert.True(result.Count == 4);
        }

        [Fact(Skip = "Not implemented")]
        public void IsImage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var listingsController = this.CreateListingsController();
            IFormFile postedFile = null;

            // Act
            //var result = listingsController.IsImage(
         //       postedFile);

            // Assert
            Assert.True(false);
        }


        public Listings GenerateListing(int id = -1, String userid = "", int subcategoryid = -1, String name = "", String description = "", int quantity = -1, short verified = 0, short display = 0, DateTime date = new DateTime())
        {
            Random rand = new Random();
            if (userid.Equals(""))
                userid = rand.Next(0, 1000000000).ToString();
            if (subcategoryid == -1)
                subcategoryid = rand.Next(0, 100000);
            if (name.Equals(""))
                name = rand.Next(0, 10000000).ToString();
            if (description.Equals(""))
                description = rand.Next(0, 10000000).ToString();
            if (quantity == -1)
                quantity = rand.Next(0, 10);
            if (id == -1)
                id = rand.Next(1, 1000000000);

            Listings listing = new Listings()
            {
                Id = id,
                Name = name,
                Subcategoryid = subcategoryid,
                Description = description,
                Quantity = quantity,
                Verified = verified,
                Display = display,
                Date = date,
                ListingPictures = new List<ListingPictures>()
            };

            return listing;
        }
    }
}
