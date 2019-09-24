using System.Threading.Tasks;

namespace Meeeeeediator.Core.Interfaces
{
    public interface IBehavior<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        Task<TReturn> HandleAsync(TQuery query, QueryHandlerDelegate<TReturn> next);
    }
}