using System.Data;

namespace Videa.Datastore.Connection
{
    public interface IDatastoreConnectionProxyFactory
    {
        IDatastoreConnectionProxy Create(IDbConnection connection, IDbTransaction transaction);
    }
}