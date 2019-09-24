using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meeeeeediator.Application.Post;
using Meeeeeediator.Application.Post.DataAccess;

namespace Meeeeeediator.IntegrationTests.Mocks
{
    public class TestPostFetcher : IPostFetcher
    {
        private static readonly ICollection<Post> _posts = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                Title = "MMM",
                Body = "ABC"
            },
            new Post()
            {
                Id = 2,
                Title = "EEE",
                Body = "XYZ"
            }
        };

        public Task<Post> GetById(int id)
        {
            return Task.FromResult(_posts.Single(x => x.Id == id));
        }

        public Task<ICollection<Post>> GetAll()
        {
            return Task.FromResult(_posts);
        }
    }
}