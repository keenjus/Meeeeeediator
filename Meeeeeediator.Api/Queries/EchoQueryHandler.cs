using System.Threading.Tasks;
using Meeeeeediator.Core;

namespace Meeeeeediator.Api.Queries
{
    public class EchoQueryHandler : IQueryHandler<EchoQuery, string>
    {
        public Task<string> HandleAsync(EchoQuery query)
        {
            return Task.FromResult(query.Message);
        }
    }
}
