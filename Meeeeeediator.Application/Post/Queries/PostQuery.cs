using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application.Post.Queries
{
    public class PostQuery : IQuery<Post>
    {
        public PostQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}