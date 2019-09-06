using System.Threading.Tasks;
using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application
{
    public class EchoQueryHandler : IQueryHandler<EchoQuery, string>
    {
        public Task<string> HandleAsync(EchoQuery query)
        {
            return Task.FromResult(query.Message?.ToUpperInvariant());
        }
    }
}