using Meeeeeediator.Core.Helpers;
using Meeeeeediator.Core.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Meeeeeediator.Core
{
    public class ProxyMediator : IMediator
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public ProxyMediator(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> SendAsync<T>(IQuery<T> query)
        {
            return await SendAsync<T>((object)query).ConfigureAwait(false);
        }

        public async Task<object> SendAsync(string name, string query)
        {
            return await SendAsync<object>(name, query).ConfigureAwait(false);
        }

        private async Task<T> SendAsync<T>(string name, string query)
        {
            var body = new StringContent(query, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/query?name={name}", body).ConfigureAwait(false);

            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);

            return _serializer.Deserialize<ResponseWrapper<T>>(jsonTextReader).Data;
        }

        private async Task<T> SendAsync<T>(object query)
        {
            string queryName = QueryHelper.GetQueryName(query.GetType());
            return await SendAsync<T>(queryName, JsonConvert.SerializeObject(query)).ConfigureAwait(false);
        }

        private class ResponseWrapper<T>
        {
            public T Data { get; set; }
        }
    }
}