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

namespace AdvertSiteTests
{
    public class TestHelpers
    {


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


        public static advert_siteContext CreateFakeDbContext()
        {
            var dbOptions = new DbContextOptionsBuilder<advert_siteContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            return new advert_siteContext(dbOptions);
        }

        public static (ApplicationUser fakeUser, ControllerContext fakeControllerContext) FakeUserAndControllerContext(advert_siteContext mockadvert_siteContext)
        {
            //setup fake user
            var fakeUser = new ApplicationUser()
            {
                Email = "test@gmail.com",
                UserName = "FakeBob"
            };
            mockadvert_siteContext.Users.Add(fakeUser);
            mockadvert_siteContext.SaveChanges();

            var identity = new GenericIdentity(fakeUser.Id, ClaimTypes.NameIdentifier);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, fakeUser.Id));
            var fakeUserIdent = new GenericPrincipal(identity, new string[] { "User" });
            //fakeUserIdent.AddIdentity(identity);
            var fakeControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeUserIdent
                }
            };
            
            //set fake user
            return (fakeUser, fakeControllerContext);
        }
    }
}
