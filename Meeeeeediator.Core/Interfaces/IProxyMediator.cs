using System.Threading.Tasks;

namespace Meeeeeediator.Core.Interfaces
{
    public interface IProxyMediator
    {
        Task<T> SendAsync<T>(IQuery<T> query);
    }
}