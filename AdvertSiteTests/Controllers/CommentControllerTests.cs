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
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace AdvertSiteTests.Controllers
{
    public class CommentControllerTests : IDisposable
    {
        private ApplicationUser fakeUser;

        private MockRepository mockRepository;

        private advert_siteContext mockadvert_siteContext;
        private UserManager<ApplicationUser> mockUserManager;
        private ControllerContext fakeControllerContext;

        public CommentControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Loose);

            var dbOptions = new DbContextOptionsBuilder<advert_siteContext>()
            .UseInMemoryDatabase(databaseName: "test")
            .Options;
            this.mockadvert_siteContext = new advert_siteContext(dbOptions);
            this.mockUserManager = TestUserManager<ApplicationUser>();


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private CommentController CreateCommentController(bool withUser)
        {
            var commentController =  new CommentController(this.mockadvert_siteContext, this.mockUserManager);
            if (withUser)
            {   //setup fake user
                fakeUser = new ApplicationUser()
                {
                    Email = "test@gmail.com"
                };
                mockadvert_siteContext.Users.Add(fakeUser);
                mockadvert_siteContext.SaveChanges();

                var identity = new GenericIdentity(fakeUser.Id, ClaimTypes.NameIdentifier);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, fakeUser.Id));
                var fakeUserIdent = new GenericPrincipal(identity, new string[] { "User" });
                //fakeUserIdent.AddIdentity(identity);
                fakeControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = fakeUserIdent
                    }
                };
                //set fake user
                commentController.ControllerContext = fakeControllerContext;
            }

            return commentController;
        }

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var commentController = this.CreateCommentController(false);
            int id = 0;

            // Act
            var result = commentController.Create(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task CreateAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var commentController = this.CreateCommentController(true);
            Comments comment = new Comments() {
                Text = "TestComment"
            };
            

            //create new listing for comment
            Listings listing = new Listings()
            {
                Name = "Test Nice Car",
                Description = "free car :)",
                Userid = fakeUser.Id
            };
            mockadvert_siteContext.Listings.Add(listing);
            mockadvert_siteContext.SaveChanges();
            //get id
            int id = listing.Id;


            // Act
            var result = (RedirectToActionResult)await commentController.CreateAsync(id, comment);
            var addedComment = await mockadvert_siteContext.Comments.FirstAsync(c => c.Userid == fakeUser.Id);

            // Assert
            Assert.Equal(addedComment.Text, comment.Text);
            Assert.Equal(addedComment.Userid, fakeUser.Id);
            
        }
        
        [Fact]
        public async Task CreateAjax_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var commentController = this.CreateCommentController(false);
            int id = 0;
            ListingAndComment listingAndComment = null;

            // Act
            var result = await commentController.CreateAjax(
                id,
                listingAndComment);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var commentController = this.CreateCommentController(false);
            int? id = null;

            // Act
            var result = commentController.Delete(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteConfirmed_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var commentController = this.CreateCommentController(false);
            int id = 0;

            // Act
            var result = await commentController.DeleteConfirmed(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetComments_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var commentController = this.CreateCommentController(false);
            int listingId = 0;

            // Act
            var result = await commentController.GetComments(
                listingId);

            // Assert
            Assert.True(false);
        }

        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }
    }
}
