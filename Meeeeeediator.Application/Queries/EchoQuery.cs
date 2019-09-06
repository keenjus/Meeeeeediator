using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application.Queries
{
    public class EchoQuery : IQuery<string>
    {
        public string Message { get; set; }
    }
}
