using Meeeeeediator.Api;
using Meeeeeediator.Application.Queries;
using Meeeeeediator.Core;
using Meeeeeediator.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Meeeeeediator.IntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task EchoQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            string expected = "Hello World. Is this working?";

            var response = await proxyMediator.SendAsync(new EchoQuery() { Message = expected });

            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task DuplicateEchoQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            string inputMessage = "hello world. is this uppercase?";
            string expected = inputMessage.ToUpperInvariant();

            var response = await proxyMediator.SendAsync(new Application.EchoQuery(inputMessage));

            Assert.Equal(expected, response);
        }

        private IProxyMediator CreateProxyMediator()
        {
            return new ProxyMediator(CreateClient());
        }

        private HttpClient CreateClient()
        {
            return _factory.CreateClient();
        }
    }
}
