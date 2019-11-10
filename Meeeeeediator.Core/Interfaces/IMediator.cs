using System.Threading.Tasks;

namespace Meeeeeediator.Core.Interfaces
{
    public interface IMediator
    {
        Task<TReturn> SendAsync<TReturn>(IQuery<TReturn> query);

        Task<object> SendAsync(string name, string query);
    }
}