using System.Net;
using System.Threading.Tasks;
using Api.IntegrationTests.Base;
using Xunit;

namespace Api.IntegrationTests
{
    public class BasicTest : IntegrationTestBase, IClassFixture<ApiAppFactory>
    {
        public BasicTest(ApiAppFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("users")]
        public async Task Unauthorized(string url)
        {
            var response = await Client.GetAsync(url);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("users")]
        public async Task Authorized(string url)
        {
            await Authorize(Client);
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WrongLoginAttempt()
        {
            var (response, _) = await Authorize(Client, "nope", "nope", false);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}