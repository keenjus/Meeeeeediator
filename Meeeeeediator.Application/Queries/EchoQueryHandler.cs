using System.Threading;
using System.Threading.Tasks;
using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application.Queries
{
    public class EchoQueryHandler : IQueryHandler<EchoQuery, string>
    {
        public async Task<string> HandleAsync(EchoQuery query)
        {
            await Task.Run(() => Thread.Sleep(900));

            return query.Message;
        }
    }
}
