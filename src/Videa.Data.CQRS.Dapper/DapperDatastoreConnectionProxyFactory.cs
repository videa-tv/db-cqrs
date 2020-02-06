using System.Data;
using Videa.Data.CQRS.Connection;

namespace Videa.Data.CQRS.Dapper
{
    public class DapperDatastoreConnectionProxyFactory : IDatastoreConnectionProxyFactory
    {
        public IDatastoreConnectionProxy Create(IDbConnection connection, IDbTransaction transaction)
        {
            return new DapperDatastoreConnectionProxy(connection, transaction);
        }
    }
}