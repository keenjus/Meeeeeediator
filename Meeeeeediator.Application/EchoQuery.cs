using Meeeeeediator.Core.Attributes;
using Meeeeeediator.Core.Interfaces;

namespace Meeeeeediator.Application
{
    /// <summary>
    /// Duplicate query by type name. Used for testing custom name functionality.
    /// </summary>
    [Query(Name = "EchoQueryV2")]
    public class EchoQuery : IQuery<string>
    {
        public EchoQuery(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
