using System;
using System.Data;
using System.Threading.Tasks;
using Videa.Datastore.Command;
using Videa.Datastore.Connection;
using Videa.Datastore.Provider;
using Videa.Datastore.Query;

namespace Videa.Datastore
{
    public interface IDatastoreContext<TDatastore> : IDisposable 
        where TDatastore : IDbProvider
    {
        IDbTransaction BeginTransaction();
        Task<int> Execute(ICommand command);
        Task<T> Query<T>(IQuery<T> asyncQuery);
    }

    public class DatastoreContext<TDatastore> : IDatastoreContext<TDatastore>
        where TDatastore : IDbProvider
    {
        protected readonly TDatastore DbProvider;
        protected readonly IDatastoreConnectionProxyFactory DatastoreConnectionProxyFactory;

        public DatastoreContext(
            TDatastore dbProvider,
            IDatastoreConnectionProxyFactory datastoreConnectionProxyFactory)
        {
            DbProvider = dbProvider == null ?
                throw new ArgumentNullException(nameof(dbProvider)): dbProvider;

            DatastoreConnectionProxyFactory = datastoreConnectionProxyFactory ?? throw new ArgumentNullException(nameof(datastoreConnectionProxyFactory));
        }

        protected IDatastoreConnectionProxy DbConnectionProxy => DatastoreConnectionProxyFactory.Create(Connection, _transaction);

        private IDbTransaction _transaction;
        private IDbConnection _connection;

        /// <summary>
        /// Get the current connection, or open a new connection
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = DbProvider.GetConnection();

                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        /// <summary>
        /// Starts a new transaction if one is not already available
        /// Then returns the transaction.
        /// </summary>
        public virtual IDbTransaction BeginTransaction()
        {
            if (_transaction?.Connection == null)
                _transaction = Connection.BeginTransaction();

            return _transaction;
        }

        /// <summary>
        /// Asynchronously Execute a command on the dapper context
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual async Task<int> Execute(ICommand command)
        {
            return await command.Execute(DbConnectionProxy).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously Execute a asyncQuery on the dapper context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncQuery"></param>
        /// <returns></returns>
        public virtual async Task<T> Query<T>(IQuery<T> asyncQuery)
        {
            return await asyncQuery.Execute(DbConnectionProxy).ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose of the transaction and close the connection
        /// </summary>
        public virtual void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}