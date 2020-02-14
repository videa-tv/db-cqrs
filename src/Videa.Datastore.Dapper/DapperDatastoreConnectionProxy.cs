using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dapper;
using Videa.Datastore.Connection;

namespace Videa.Datastore.Dapper
{
    [ExcludeFromCodeCoverage]
    public class DapperDatastoreConnectionProxy : IDatastoreConnectionProxy
    {
        protected readonly IDbConnection DbConnection;
        protected readonly IDbTransaction DbTransaction;

        public DapperDatastoreConnectionProxy(IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            DbTransaction = dbTransaction;
        }

        public IDbConnection ProxyDbConnection => DbConnection;

        public async Task<int> Execute(string sql, object param, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await DbConnection
                .ExecuteAsync(sql, param,  DbTransaction,  commandTimeout, commandType)
                .ConfigureAwait(false);
        }

        public async Task<T> QueryFirstOrDefault<T>(string query, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await DbConnection
                .QueryFirstOrDefaultAsync<T>(query, param, DbTransaction, commandTimeout, commandType)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Query<T>(string query, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await DbConnection
                .QueryAsync<T>(query, param, DbTransaction, commandTimeout, commandType)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Query<T>(string query, bool buffered, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return buffered
                ? await Query<T>(query, param, commandTimeout, commandType)
                : await Task.FromResult(DbConnection.Query<T>(
                    query,
                    param,
                    commandType: commandType,
                    transaction: DbTransaction, buffered: false));
        }
    }
}