using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ActivityAssistent.Shared.Dtos.ActionPoints;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ActivityAssistent.Api.IntegrationTests
{
    public class ActionPointAuthorizationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ActionPointAuthorizationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async global::System.Threading.Tasks.Task CreateActionPoint_WhenUnauthorized_Returns401()
        {
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/ActionPoint/create", new CreateActionPointDto
            {
                ConversationId = System.Guid.NewGuid(),
                Description = "Unauthorized test action point",
                SalesUserId = System.Guid.NewGuid(),
                DueDate = System.DateTime.UtcNow.AddDays(1),
                IsCompleted = false,
                SubNrId = System.Guid.NewGuid()
            });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
