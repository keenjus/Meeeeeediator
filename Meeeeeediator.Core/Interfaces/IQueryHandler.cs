using System.Threading.Tasks;

namespace Meeeeeediator.Core.Interfaces
{
    public interface IQueryHandler<in TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
        Task<TReturn> HandleAsync(TQuery query);
    }
}