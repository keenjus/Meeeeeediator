using System.Collections.Generic;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Post.DataAccess
{
    public interface IPostFetcher
    {
        Task<Post> GetById(int id);
        Task<ICollection<Post>> GetAll();
    }
}