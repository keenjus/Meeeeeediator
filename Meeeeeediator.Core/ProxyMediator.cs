using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Meeeeeediator.Core
{
    public class ProxyMediator : IProxyMediator
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public ProxyMediator(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> SendAsync<T>(IQuery<T> query)
        {
            var body = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");

            string queryName = query.GetType().Name;

            var response = await _client.PostAsync($"/query?name={queryName}", body);

            var stream = await response.Content.ReadAsStreamAsync();

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var parsed = _serializer.Deserialize<ResponseWrapper<T>>(jsonTextReader);
                return parsed.Data;
            }
        }

        private class ResponseWrapper<T>
        {
            public T Data { get; set; }
        }
    }

    public interface IProxyMediator
    {
        Task<T> SendAsync<T>(IQuery<T> query);
    }
}