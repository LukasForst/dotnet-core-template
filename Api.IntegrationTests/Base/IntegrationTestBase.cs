using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DotnetApp.Configuration;
using DotnetApp.DTO.Auth;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.IntegrationTests.Base
{
    /// <summary>
    ///     Base for the integration tests, which contains http client connected to the application.
    /// </summary>
    public abstract class IntegrationTestBase
    {
        protected readonly HttpClient Client;
        protected readonly WebApplicationFactory<Startup> Factory;

        protected IntegrationTestBase(WebApplicationFactory<Startup> factory)
        {
            Factory = factory;
            var client = factory.CreateClient();
            Client = client;
        }

        /// <summary>
        ///     Performs authorization call and sets header to client.
        /// </summary>
        protected async Task<(HttpResponseMessage, TokenDto)> Authorize(
            HttpClient? httpClient,
            string username = "admin", string password = "hello",
            bool ensure = true
        )
        {
            var client = httpClient ?? Client;

            var response = await client.PostAsJsonAsync("Users/authenticate", new LoginRequest
            {
                Username = username,
                Password = password
            });

            if (ensure) response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<TokenDto>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);
                return (response, model);
            }

            return (response, null);
        }
    }
}