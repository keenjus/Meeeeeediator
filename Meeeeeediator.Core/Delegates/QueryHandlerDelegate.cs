using System.Threading.Tasks;

namespace Meeeeeediator.Core.Delegates
{
    public delegate Task<TReturn> QueryHandlerDelegate<TReturn>();
}
