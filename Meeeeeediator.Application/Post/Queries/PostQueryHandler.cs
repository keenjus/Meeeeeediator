using System.Linq;
using System.Threading.Tasks;
using Meeeeeediator.Application.Post.DataAccess;
using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application.Post.Queries
{
    public class PostQueryHandler : IQueryHandler<PostQuery, Post>
    {
        private readonly IPostFetcher _fetcher;

        public PostQueryHandler(IPostFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public async Task<Post> HandleAsync(PostQuery query)
        {
            var posts = await _fetcher.GetPosts();
            return posts.Single(x => x.Id == query.Id);
        }
    }
}