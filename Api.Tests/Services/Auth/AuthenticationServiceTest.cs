using System.Threading.Tasks;
using Api.Services.Auth;
using DataAccess.Context;
using DataAccess.Model;
using Xunit;

namespace Api.Tests.Services.Auth
{
    public class AuthenticationServiceDatabaseTest : DatabaseTestBase
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

        [Fact]
        public async Task AuthServiceShouldReturnNull_UserDoesNotExist()
        {
            var authService = GetService<AuthenticationService>();
            var token = await authService.Authenticate("john-doe-3", "hello-wrong");

            Assert.Null(token);
        }

        [Fact]
        public async Task AuthServiceShouldReturnNull_WrongPassword()
        {
            var authService = GetService<AuthenticationService>();
            var token = await authService.Authenticate("john-doe", "hello-wrong");

            Assert.Null(token);
        }


        [Fact]
        public async Task AuthServiceShouldReturnValidToken()
        {
            var authService = GetService<AuthenticationService>();
            var token = await authService.Authenticate("john-doe", "hello");

            Assert.NotNull(token);
            Assert.NotEmpty(token!.Token);
        }
    }
}