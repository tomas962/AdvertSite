using AdvertSite.Controllers;
using AdvertSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
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
            this.context.Database.EnsureDeleted();
        }

        private ListingsController CreateListingsController(Boolean withUser = false)
        {
            var listingController = new ListingsController(
                this.context, this.mockUserManager);
            if (withUser)
            {
                (fakeUser, listingController.ControllerContext) = TestHelpers.FakeUserAndControllerContext(context);
            }

            return listingController;

        }

        [Theory]
        [InlineData("Subcategory", null, 10, true, 5)]
        [InlineData("Category", null, 5, true, 7)]
        [InlineData("Search", "mouse", null, true, 3)]
        [InlineData("MyListings", null, null, true, 2)]
        [InlineData("randomstringype", "randomkeytype", 15666, true, 9)]
        [InlineData(null, null, null, true, 9)]
        public async Task Index_RequestQuery_OpenAllListingView(string type, string key, int? id, bool loggedIn, int expectedResult)
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Query["type"]).Returns(type);
            httpContext.Setup(x => x.Request.Query["key"]).Returns(key);
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };

            var listingsController = this.CreateListingsController(loggedIn);
            var category = new Category()
            {
                Id = 5,
                Name = "cars"
            };
            var subcategory = new Subcategory()
            {
                Id = 10,
                Categoryid = 5,
                Name = "BMW"
            };
            var subcategory2 = new Subcategory()
            {
                Id = 11,
                Categoryid = 5,
                Name = "Audi"
            };
            context.Category.Add(category);
            context.Subcategory.Add(subcategory);
            context.Subcategory.Add(subcategory2);
            await context.SaveChangesAsync();

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(subcategoryid: 4, verified: 1, display: 1, name: "mouse"),
                GenerateListing(subcategoryid: 10, verified: 1, display: 1),
                GenerateListing(subcategoryid: 10, verified: 1, display: 1, name: "keyboard"),
                GenerateListing(subcategoryid: 10, verified: 1, display: 1),
                GenerateListing(subcategoryid: 10, verified: 1, display: 1),
                GenerateListing(subcategoryid: 10, verified: 1, display: 1, name: "Mouse123"),
                GenerateListing(subcategoryid: 3, verified: 1, display: 1, name: "ASDASDASD"),
                GenerateListing(subcategoryid: 11, verified: 1, display: 1, name: "test", description: "good mouse"),
                GenerateListing(subcategoryid: 11, verified: 1, display: 1),
                GenerateListing(subcategoryid: 11, verified: 0, display: 0),
            };
            if (loggedIn)
            {
                list[0].Userid = fakeUser.Id;
                list[1].Userid = fakeUser.Id;
            }
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            listingsController.ControllerContext = controllerContext;

            var result = await listingsController.Index(id);
            var viewResult = (ViewResult)result;
            var listings = (List<Listings>)viewResult.Model;
            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedResult, listings.Count);
        }

        [Fact]
        public async Task UncomfirmedListings_Listings_ReturnUnconfirmedListingView()
        {
            // Arrange
            var listingsController = this.CreateListingsController();

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 1),
                GenerateListing(verified: 1),
                GenerateListing(verified: 0),
            };
            int unconfirmedCount = list.FindAll(x => x.Verified == 0).Count;
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();
            
            var result = await listingsController.UncomfirmedListings();
            var viewResult = (ViewResult)result;
            var unconfirmedListings = (List<Listings>)viewResult.Model;


            Assert.IsType<ViewResult>(result);
            Assert.Equal(unconfirmedCount, unconfirmedListings.Count);
        }

        [Fact]
        public async Task Details_UserListing_ListingDetailsView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            var category = new Category() { Name = "Test", Id = 5 };
            var subcategory = new Subcategory() { Name = "TESTAS", Id = 5, Categoryid = 5 };
            context.Category.Add(category);
            context.Subcategory.Add(subcategory);
            await context.SaveChangesAsync();

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0, subcategoryid: 5, userid: this.fakeUser.Id),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 1,  userid: this.fakeUser.Id),
                GenerateListing(verified: 1),
                GenerateListing(verified: 0),
            };
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Details(list[0].Id);
            var listingView = (ViewResult)result;
            var listing = (ListingAndComment)listingView.Model;

            Assert.IsType<ViewResult>(result);
            // Assert
            Assert.Equal(listing.Listing.Id, list[0].Id);
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(null)]
        public async Task Details_UserListing_OpenNotFoundView(int? id)
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0, subcategoryid: 1, userid: this.fakeUser.Id),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 1,  userid: this.fakeUser.Id),
                GenerateListing(verified: 1),
                GenerateListing(verified: 0),
            };
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Details(id);

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void Create_UserListing_OpenCreationView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            ListingNewModel listingModel = new ListingNewModel();

            var result = listingsController.Create(listingModel);
            var viewResult = (ViewResult)result;

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CreatePost_NewListingModel_CreateAndRedirectToView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            ListingNewModel newListing = new ListingNewModel()
            {
                Subcategoryid = 0,
                Name = "Great bike",
                Description = "Greatest bike there is",
                Price = 500.0,
            };


            var result = await listingsController.CreatePost(newListing);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task CreatePost_InvalidModelState_OpenView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            listingsController.ModelState.AddModelError("key", "error");

            var result = await listingsController.CreatePost(null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Edit_UserListingId_OpenEditView()
        {
            var listingsController = this.CreateListingsController(true);
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(userid: this.fakeUser.Id),
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Edit(list[2].Id);
            var viewResult = (ViewResult)result;
            var toEditInfo = (Listings)viewResult.Model;

            Assert.IsType<ViewResult>(result);
            Assert.True(toEditInfo.Id == list[2].Id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(null)]
        public async Task Edit_UserListingId_OpenNotFoundView(int? id)
        {
            var listingsController = this.CreateListingsController(true);
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(userid: this.fakeUser.Id),
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Edit(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_UserListingId_OpenForbidView()
        {
            var listingsController = this.CreateListingsController(true);
            var user = new ApplicationUser()
            {
                UserName = "Frank",
                Email = "TheGreatesFrank@gmail.com"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(userid: user.Id),
                GenerateListing(userid: this.fakeUser.Id),
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Edit(list[0].Id);

            Assert.IsType<ForbidResult>(result);
        }


        [Fact]
        public async Task Edit_UserListing_ChangeListing()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);       
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing()
            };
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            list[0].Description = "New Desciption";
            list[0].Name = "New Name";

            var result = await listingsController.Edit(list[0].Id, list[0]);
            var editedListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(list[0].Name, editedListing.Name);
            Assert.Equal(list[0].Description, editedListing.Description);
            Assert.True(list[0].Verified == 0);
            Assert.True(list[0].Display == 1);

        }

        [Fact]
        public async Task Edit_UserListingDifferentIds_OpenNotFoundView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(userid: this.fakeUser.Id),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing()
            };
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            list[0].Description = "New Desciption";
            list[0].Name = "New Name";

            var result = await listingsController.Edit(list[0].Id + 1, list[0]);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_NotYourListing_OpenForbidView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            var user = new ApplicationUser()
            {
                UserName = "Steven",
                Email = "Steven@gmail.com"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(userid: user.Id),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing()
            };
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            list[0].Description = "New Desciption";
            list[0].Name = "New Name";

            var result = await listingsController.Edit(list[0].Id, list[0]);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task Delete_UserListing_OpenView()
        {
            // Arrange
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

            var result = await listingsController.Delete(list[2].Id);
            var viewResult = (ViewResult)result;
            var deleteInfo = (Listings)viewResult.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.True(deleteInfo.Id == list[2].Id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(null)]
        public async Task Delete_UserListing_OpenNotFoundView(int? id)
        {
            // Arrange
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(),
                GenerateListing(),
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_UserListing_DeleteListing()
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

            var result = await listingsController.DeleteConfirmed(list[0].Id);
            var deletedListing = await context.Listings.FindAsync(list[0].Id);
            string type = result.GetType().ToString();

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(deletedListing);
        }

        [Fact]
        public async Task DenyListing_UserListing_DenyListing()
        {
            // Arrange
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.DenyListing(list[0].Id);

            var deniedListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(deniedListing);
        }
        [Fact]
        public async Task DenyListing_UserListingNotAdmin_OpenForbidView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.DenyListing(list[0].Id);

            var deniedListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<ForbidResult>(result);
            Assert.NotNull(deniedListing);
        }

        [Fact]
        public async Task ApproveListing_UserListing_MarkAsApproved()
        {
            // Arrange
            var listingsController = this.CreateListingsController();
            var user = new ApplicationUser() { UserName = "Adam", Email = "test@gamil.com" };

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 0),
                GenerateListing(verified: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.ApproveListing(list[0].Id);

            var approvedListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(approvedListing.Verified == 1);
        }

        [Fact]
        public async Task ApproveListing_UserListingNotAdmin_OpenForbidView()
        {
            // Arrange
            var listingsController = this.CreateListingsController(true);

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(verified: 0),
                GenerateListing(verified: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.ApproveListing(list[0].Id);

            var approvedListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<ForbidResult>(result);
            Assert.True(approvedListing.Verified == 0);
        }

        [Fact]
        public async Task Hide_UserListing_MakeHidden()
        {
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(display: 0),
                GenerateListing(display: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Hide(list[0].Id);

            var hiddenListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(hiddenListing.Display == 0);
        }
        
        [Fact]
        public async Task Hide_UserListingInvalidUser_OpenNotFoundView()
        {
            var listingsController = this.CreateListingsController();

            List<Listings> list = new List<Listings>()
            {
                GenerateListing(display: 0, userid: "test"),
                GenerateListing(display: 0)
            };

            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            var result = await listingsController.Hide(list[0].Id);

            var hiddenListing = await context.Listings.FindAsync(list[0].Id);

            Assert.IsType<NotFoundResult>(result);
            Assert.True(hiddenListing.Display == 1);
        }


        [Fact]
        public async Task ListingsJSON_ListingsInDatabase_ReturnJson()
        {
            // Arrange
            var listingsController = this.CreateListingsController();
            List<Listings> list = new List<Listings>()
            {
                GenerateListing(),
                GenerateListing(),
                GenerateListing(),
                GenerateListing()

            };
            context.Listings.AddRange(list);
            await context.SaveChangesAsync();

            // Act
            var result = await listingsController.ListingsJSON();
            var jsonResult = (JsonResult)result;

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Theory]
        [InlineData(5555, true)]
        [InlineData(2, false)]
        [InlineData(4501788, true)]
        public void ListingsExists_ListingId_ReturnBool(int id, bool expectedResponse)
        {
            var listingsController = this.CreateListingsController(true);
            List<Listings> list = new List<Listings>()
            {
                GenerateListing( id: 5555),
                GenerateListing( id: 463768),
                GenerateListing( id: 456772),
                GenerateListing( id: 4501788),
                GenerateListing( id: 4567871),
                GenerateListing( id: 54728767),
            };

            foreach (var item in list)
            {
                context.Listings.Add(item);
            }
            context.SaveChanges();

            bool result = listingsController.ListingsExists(id);

            Assert.Equal(expectedResponse, result);
        }

        [Theory]
        [InlineData("TEST_PICTURE.png", true)]
        [InlineData("WRONG_FILE.txt", false)]
        public void IsImage_StateUnderTest_ExpectedBehavior(string imageName, bool expectedResult)
        {
            // Arrange
            var picStream = new FileStream("../../../TestUserPictures/" + imageName, FileMode.Open);

            var image = new FormFile(picStream, 0, picStream.Length, imageName, imageName)
            {
                Headers = new HeaderDictionary(),
                ContentType = new string("image/png")
            };

           

            var result = ListingsController.IsImage(image);

            picStream.Close();
            Assert.Equal(expectedResult, result);
        }

        public Listings GenerateListing(int id = -1, String userid = null, int subcategoryid = -1, String name = "", String description = "", int quantity = -1, short verified = 0, short display = 0, DateTime date = new DateTime())
        {
            Random rand = new Random();
            if (userid == null)
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
                Userid = userid,
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
