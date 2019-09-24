using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Post.DataAccess
{
    public class PostFetcher : IPostFetcher
    {
        private readonly HttpClient _httpClient;

        public PostFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Post> GetById(int id)
        {
            return (await GetAll()).Single(x => x.Id == id);
        }

        public async Task<ICollection<Post>> GetAll()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
            return JsonConvert.DeserializeObject<List<Post>>(await response.Content.ReadAsStringAsync());
        }
    }
}