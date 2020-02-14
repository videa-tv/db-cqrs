using Videa.Datastore.Connection;
using System.Threading.Tasks;

namespace Videa.Datastore.Query
{
    public interface IQuery<T>
    {
        Task<T> Execute(IDatastoreConnectionProxy datastoreConnectionProxy);
    }
}