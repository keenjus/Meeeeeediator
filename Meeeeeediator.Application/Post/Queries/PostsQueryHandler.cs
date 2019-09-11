using Meeeeeediator.Application.Post.DataAccess;
using Meeeeeediator.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Post.Queries
{
    public class PostsQueryHandler : IQueryHandler<PostsQuery, ICollection<Post>>
    {
        private readonly IPostFetcher _fetcher;

        public PostsQueryHandler(IPostFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public async Task<ICollection<Post>> HandleAsync(PostsQuery query)
        {
            return await _fetcher.GetPosts();
        }
    }
}