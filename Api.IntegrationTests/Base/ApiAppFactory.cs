using DotnetApp.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.IntegrationTests.Base
{
    /// <summary>
    ///     Factory which starts up the application.
    /// </summary>
    public class ApiAppFactory : WebApplicationFactory<Startup>
    {
    }
}