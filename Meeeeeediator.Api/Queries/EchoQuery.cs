using Meeeeeediator.Core;

namespace Meeeeeediator.Api.Queries
{
    public class EchoQuery : IQuery<string>
    {
        public string Message { get; set; }
    }
}
