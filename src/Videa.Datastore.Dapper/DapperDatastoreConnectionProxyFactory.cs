using System.Data;
using Videa.Datastore.Connection;

namespace Videa.Datastore.Dapper
{
    public class DapperDatastoreConnectionProxyFactory : IDatastoreConnectionProxyFactory
    {
        public IDatastoreConnectionProxy Create(IDbConnection connection, IDbTransaction transaction)
        {
            return new DapperDatastoreConnectionProxy(connection, transaction);
        }
    }
}