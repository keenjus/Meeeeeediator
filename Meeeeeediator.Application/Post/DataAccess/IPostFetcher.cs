using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Post.DataAccess
{
    public interface IPostFetcher
    {
        Task<ICollection<Post>> GetPosts();
    }
}