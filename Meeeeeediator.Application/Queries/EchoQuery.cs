using Meeeeeediator.Core;

namespace Meeeeeediator.Application.Queries
{
    public class EchoQuery : IQuery<string>
    {
        public string Message { get; set; }
    }
}
