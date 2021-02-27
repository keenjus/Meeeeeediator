using Bogus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeeeeediator.Application.Post.DataAccess
{
    public class PostFetcher : IPostFetcher
    {
        private readonly Faker<Post> _faker;
        private readonly List<Post> _posts;

        public PostFetcher()
        {
            _faker = new Faker<Post>()
                .RuleFor(x => x.Id, (f, p) => f.UniqueIndex)
                .RuleFor(x => x.Title, (f, p) => f.Lorem.Sentence())
                .RuleFor(x => x.Body, (f, p) => f.Lorem.Text());

            _posts = _faker.Generate(20);
        }

        public Task<Post> GetById(int id) => Task.FromResult(_posts.Single(x => x.Id == id));

        public Task<ICollection<Post>> GetAll() => Task.FromResult(_posts as ICollection<Post>);
    }
}