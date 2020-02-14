using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Videa.Datastore.Connection
{
    public interface IDatastoreConnectionProxy
    {
        IDbConnection ProxyDbConnection { get; }
        Task<int> Execute(string sql, object param, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QueryFirstOrDefault<T>(string query, object param = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<T>> Query<T>(string query, object param = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<T>> Query<T>(string query, bool buffered, object param = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}