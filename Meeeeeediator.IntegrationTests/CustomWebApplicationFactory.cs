using Meeeeeediator.Api;
using Meeeeeediator.Application.Post.DataAccess;
using Meeeeeediator.IntegrationTests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Meeeeeediator.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IPostFetcher, TestPostFetcher>();
            });
        }
    }
}