using System.Linq;
using Meeeeeediator.Application.Queries;
using Meeeeeediator.Core;
using Meeeeeediator.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using Meeeeeediator.Application.Post.Queries;
using Newtonsoft.Json;
using Xunit;

namespace Meeeeeediator.IntegrationTests
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public BasicTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            int postId = 1;

            var response = await proxyMediator.SendAsync(new PostQuery(postId));

            Assert.NotNull(response);

            Assert.Equal(postId, response.Id);
        }

        [Fact]
        public async Task PostsQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            var response = await proxyMediator.SendAsync(new PostsQuery());

            Assert.NotNull(response);

            Assert.True(response.Any());
        }

        [Fact]
        public async Task EchoQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            string expected = "Hello World. Is this working?";

            string response = await proxyMediator.SendAsync(new EchoQuery() { Message = expected });

            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task DuplicateEchoQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            string inputMessage = "hello world. is this uppercase?";
            string expected = inputMessage.ToUpperInvariant();

            string response = await proxyMediator.SendAsync(new Application.EchoQuery(inputMessage));

            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task DynamicObjectQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            string inputMessage = "hello world. is this uppercase?";
            string expected = inputMessage.ToUpperInvariant();

            var response = await proxyMediator.SendAsync((object)new Application.EchoQuery(inputMessage));

            Assert.Equal(expected, response);
        }

        [Fact]
        public async Task DynamicJsonifiedQueryReturnsSuccessfully()
        {
            var proxyMediator = CreateProxyMediator();

            string inputMessage = "hello world. is this uppercase?";
            string expected = inputMessage.ToUpperInvariant();

            var response = await proxyMediator.SendAsync("EchoQueryV2", JsonConvert.SerializeObject(new Application.EchoQuery(inputMessage)));

            Assert.Equal(expected, response);
        }

        private IMediator CreateProxyMediator()
        {
            return new ProxyMediator(CreateClient());
        }

        private HttpClient CreateClient()
        {
            return _factory.CreateClient();
        }
    }
}
