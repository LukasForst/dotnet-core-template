using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Services.Auth;
using DataAccess.Context;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Api.Tests.Services.Auth
{
    public class PrincipalServiceTest : DatabaseTestBase
    {
        protected override void SeedDatabase(AppDbContext context)
        {
            base.SeedDatabase(context);
            // hash corresponds to password "hello"
            context.Users.Add(
                new User(
                    "John",
                    "Doe",
                    "john-doe",
                    "1000.PpqwnVxXYtMKIjc7SZ9jdA==.ikYDT+YSJLn9c3Slw4fC61MisaBgYvVaTqcGZVw875c=",
                    "admin"
                ));
            context.SaveChanges();
        }

        private class FakePrincipal : IIdentity
        {
            public string? AuthenticationType => throw new Exception("This method should never be called in the test!");
            public bool IsAuthenticated => throw new Exception("This method should never be called in the test!");
            public string? Name => throw new Exception("This method should never be called in the test!");
        }

        [Fact]
        public async Task GetUserForClaimsShouldReturnUser()
        {
            var userTask = DbContext.Users.FirstAsync();

            var service = GetService<PrincipalService>();
            var claimIdentityMock = new Mock<ClaimsIdentity>();

            var user = await userTask;
            claimIdentityMock.Setup(x => x.Name).Returns(user.Id.ToString());

            var principal = new ClaimsPrincipal(claimIdentityMock.Object);
            var returnedUser = await service.GetUserForClaims(principal);

            Assert.Equal(user, returnedUser);
        }

        [Fact]
        public async Task GetUserForClaimsShouldThrowException_NameIsNotInt()
        {
            var service = GetService<PrincipalService>();
            var claimIdentityMock = new Mock<ClaimsIdentity>();
            claimIdentityMock.Setup(x => x.Name).Returns("not a number");

            var principal = new ClaimsPrincipal(claimIdentityMock.Object);
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GetUserForClaims(principal));
            Assert.Equal("It was not possible to obtain user id from the principal identity!", exception.Message);
        }

        [Fact]
        public async Task GetUserForClaimsShouldThrowException_PrincipalIdentityIsNotClaimsIdentity()
        {
            var service = GetService<PrincipalService>();

            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(x => x.Identity).Returns(new FakePrincipal());
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GetUserForClaims(principal.Object));
            Assert.Equal("It was not possible to obtain principal identity!", exception.Message);
        }
    }
}