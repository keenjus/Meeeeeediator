using Meeeeeediator.Core.Helpers;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Meeeeeediator.Core.Interfaces;

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

            string queryName = QueryHelper.GetQueryName(query.GetType());

            var response = await _client.PostAsync($"/query?name={queryName}", body);

            var stream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);

            return _serializer.Deserialize<ResponseWrapper<T>>(jsonTextReader).Data;
        }

        private class ResponseWrapper<T>
        {
            public T Data { get; set; }
        }
    }
}