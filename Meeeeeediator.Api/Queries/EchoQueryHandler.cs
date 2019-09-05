using System.Threading;
using System.Threading.Tasks;
using Meeeeeediator.Core;

namespace Meeeeeediator.Api.Queries
{
    public class EchoQueryHandler : IQueryHandler<EchoQuery, string>
    {
        public async Task<string> HandleAsync(EchoQuery query)
        {
            await Task.Run(() => Thread.Sleep(1000));

            return query.Message;
        }
    }
}
