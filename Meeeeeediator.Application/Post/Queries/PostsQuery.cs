using Meeeeeediator.Core.Interfaces;
using System.Collections.Generic;

namespace Meeeeeediator.Application.Post.Queries
{
    public class PostsQuery : IQuery<ICollection<Post>>, ICacheable
    {
        public string CacheKey => $"{nameof(PostsQuery)}";
    }
}