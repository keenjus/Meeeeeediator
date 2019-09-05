using Meeeeeediator.Api.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Meeeeeediator.Client.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region Dependency Injection
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient("MediatorApi", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });

            serviceCollection.AddSingleton<IProxyMediator, ProxyMediator>(sp => new ProxyMediator(sp.GetRequiredService<IHttpClientFactory>().CreateClient("MediatorApi")));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            #endregion

            var proxyMediator = serviceProvider.GetRequiredService<IProxyMediator>();

            string response = await proxyMediator.SendAsync(new EchoQuery() { Message = "Hello World" });

            Console.WriteLine(response);
        }
    }
}
