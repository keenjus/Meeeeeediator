using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application.Post.Queries
{
    public class PostQuery : IQuery<Post>, ICacheable
    {
        public PostQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public string CacheKey => $"{nameof(PostQuery)}-{Id}";
    }
}