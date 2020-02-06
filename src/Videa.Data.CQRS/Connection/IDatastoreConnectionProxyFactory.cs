using System.Data;

namespace Videa.Data.CQRS.Connection
{
    public interface IDatastoreConnectionProxyFactory
    {
        IDatastoreConnectionProxy Create(IDbConnection connection, IDbTransaction transaction);
    }
}