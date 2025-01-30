using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;
using Xunit;

namespace TaskManager.Tests.Integration
{
    // Clase base para pruebas de integración
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;

        public IntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
    }
}
