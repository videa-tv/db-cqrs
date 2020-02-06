using System.Threading.Tasks;
using Videa.Data.CQRS.Connection;

namespace Videa.Data.CQRS.Query
{
    public interface IQuery<T>
    {
        Task<T> Execute(IDatastoreConnectionProxy datastoreConnectionProxy);
    }
}