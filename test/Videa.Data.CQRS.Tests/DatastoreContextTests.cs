using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Data.CQRS.Connection;
using Videa.Data.CQRS.Provider;


namespace Videa.Data.CQRS.Tests
{
    [TestClass]
    public class DatastoreContextTests
    {
        private Mock<IDbProvider> _dbProviderMock;
        private Mock<IDatastoreConnectionProxyFactory> _datastoreConnectionProxyFactoryMock;

        private DatastoreContext<IDbProvider> _context;

        [TestInitialize]
        public void Initialize()
        {
            _dbProviderMock = new Mock<IDbProvider>();
            _datastoreConnectionProxyFactoryMock = new Mock<IDatastoreConnectionProxyFactory>();

            _context = new DatastoreContext<IDbProvider>(
                _dbProviderMock.Object, _datastoreConnectionProxyFactoryMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Null_DbProvider()
        {
            try
            {
                _context = new DatastoreContext<IDbProvider>(
                    null, _datastoreConnectionProxyFactoryMock.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "dbProvider");
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Null_DatastoreConnectionProxyFactory()
        {
            try
            {
                _context = new DatastoreContext<IDbProvider>(
                    _dbProviderMock.Object, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "datastoreConnectionProxyFactory");
                throw;
            }
        }

        [TestMethod]
        public void DatastoreContext_BeginTransaction_MockedConnection_NullTransaction()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(() => null);

            var transaction = _context.BeginTransaction();

            Assert.IsNull(transaction);

            _dbProviderMock.VerifyAll();

            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            dbConnectionMock.VerifyAll();
        }

        [TestMethod]
        public void DatastoreContext_BeginTransaction_MockedConnection_TransactionMock()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(transactionMock.Object);

            var transaction = _context.BeginTransaction();

            Assert.AreEqual(transactionMock.Object, transaction);

            _dbProviderMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            transactionMock.VerifyAll();
        }

        [TestMethod]
        public async Task DatastoreContext_ExecuteAsync_MockedConnection_NoTransaction_MockedConnectionProxy()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var datastoreConnectionProxyMock = new Mock<IDatastoreConnectionProxy>();

            _datastoreConnectionProxyFactoryMock.Setup(
                    r => r.Create(
                        It.Is<IDbConnection>(x => x == dbConnectionMock.Object)
                        , It.Is<IDbTransaction>(x => x == null)))
                .Returns(datastoreConnectionProxyMock.Object);

            var command = new Mock<Command.ICommand>();

            command.Setup(r =>
                    r.Execute(It.Is<IDatastoreConnectionProxy>(x => x == datastoreConnectionProxyMock.Object)))
                .ReturnsAsync(2);

            var result = await _context.Execute(command.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            datastoreConnectionProxyMock.VerifyAll();
            command.VerifyAll();
        }

        [TestMethod]
        public async Task DatastoreContext_ExecuteAsync_MockedConnection_MockedTransaction_NullConnectionProxy()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(transactionMock.Object);


            _datastoreConnectionProxyFactoryMock.Setup(
                    r => r.Create(
                        It.Is<IDbConnection>(x => x == dbConnectionMock.Object)
                        , It.Is<IDbTransaction>(x => x == transactionMock.Object)))
                .Returns(() => null);

            var command = new Mock<Command.ICommand>();

            command.Setup(r =>
                    r.Execute(It.IsAny<IDatastoreConnectionProxy>()))
                .ReturnsAsync(1);

            //Start transaction
            _context.BeginTransaction();
            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            var result = await _context.Execute(command.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            transactionMock.VerifyAll();
            command.VerifyAll();
        }

        [TestMethod]
        public async Task DatastoreContext_ExecuteAsync_MockedConnection_MockedTransaction_MockedConnectionProxy()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(transactionMock.Object);

            var datastoreConnectionProxyMock = new Mock<IDatastoreConnectionProxy>();

            _datastoreConnectionProxyFactoryMock.Setup(
                    r => r.Create(
                        It.Is<IDbConnection>(x => x == dbConnectionMock.Object)
                        , It.Is<IDbTransaction>(x => x == transactionMock.Object)))
                .Returns(datastoreConnectionProxyMock.Object);

            var command = new Mock<Command.ICommand>();

            command.Setup(r =>
                    r.Execute(It.Is<IDatastoreConnectionProxy>(x => x == datastoreConnectionProxyMock.Object)))
                .ReturnsAsync(1);

            //Start transaction
            _context.BeginTransaction();
            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            var result = await _context.Execute(command.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            transactionMock.VerifyAll();
            datastoreConnectionProxyMock.VerifyAll();
            command.VerifyAll();
        }

        [TestMethod]
        public async Task DatastoreContext_QueryAsync_MockedConnection_NoTransaction_MockedConnectionProxy()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var datastoreConnectionProxyMock = new Mock<IDatastoreConnectionProxy>();

            _datastoreConnectionProxyFactoryMock.Setup(
                    r => r.Create(
                        It.Is<IDbConnection>(x => x == dbConnectionMock.Object)
                        , It.Is<IDbTransaction>(x => x == null)))
                .Returns(datastoreConnectionProxyMock.Object);

            var query = new Mock<Query.IQuery<int>>();

            query.Setup(r =>
                    r.Execute(It.Is<IDatastoreConnectionProxy>(x => x == datastoreConnectionProxyMock.Object)))
                .ReturnsAsync(2);

            var result = await _context.Query(query.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            datastoreConnectionProxyMock.VerifyAll();
            query.VerifyAll();
        }

        [TestMethod]
        public async Task DatastoreContext_QueryAsync_MockedConnection_MockedTransaction_NullConnectionProxy()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(transactionMock.Object);

            _datastoreConnectionProxyFactoryMock.Setup(
                    r => r.Create(
                        It.Is<IDbConnection>(x => x == dbConnectionMock.Object)
                        , It.Is<IDbTransaction>(x => x == transactionMock.Object)))
                .Returns(() => null);

            var query = new Mock<Query.IQuery<int>>();

            query.Setup(r =>
                    r.Execute(It.IsAny<IDatastoreConnectionProxy>()))
                .ReturnsAsync(1);

            //Start transaction
            _context.BeginTransaction();
            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            var result = await _context.Query(query.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            transactionMock.VerifyAll();
            query.VerifyAll();
        }

        [TestMethod]
        public async Task DatastoreContext_QueryAsync_MockedConnection_MockedTransaction_MockedConnectionProxy()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(transactionMock.Object);

            var datastoreConnectionProxyMock = new Mock<IDatastoreConnectionProxy>();

            _datastoreConnectionProxyFactoryMock.Setup(
                r => r.Create(
                    It.Is<IDbConnection>(x => x == dbConnectionMock.Object)
                    , It.Is<IDbTransaction>(x => x == transactionMock.Object)))
                    .Returns(datastoreConnectionProxyMock.Object);

            var query = new Mock<Query.IQuery<int>>();

            query.Setup(r =>
                    r.Execute(It.Is<IDatastoreConnectionProxy>(x => x == datastoreConnectionProxyMock.Object)))
                .ReturnsAsync(1);

            //Start transaction
            _context.BeginTransaction();
            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            var result = await _context.Query(query.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            transactionMock.VerifyAll();
            datastoreConnectionProxyMock.VerifyAll();
            query.VerifyAll();
        }

        [TestMethod]
        public void DatastoreContext_Dispose()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task DatastoreContext_Dispose_Connection_NoTransaction()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            var command = new Mock<Command.ICommand>();
            command.Setup(r => r.Execute(It.IsAny<IDatastoreConnectionProxy>()))
                .ReturnsAsync(1);

            var result = await _context.Execute(command.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            _context.Dispose();

            dbConnectionMock
                .Verify(r =>
                    r.BeginTransaction(), Times.Never());

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            dbConnectionMock.Verify(r => r.Close(), Times.Once);
            transactionMock.Verify(r => r.Dispose(), Times.Never);
        }

        [TestMethod]
        public async Task DatastoreContext_Dispose_Connection_Transaction()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            _dbProviderMock.Setup(r => r.GetConnection())
                .Returns(dbConnectionMock.Object);

            var transactionMock = new Mock<IDbTransaction>();

            dbConnectionMock
                .Setup(r => r.BeginTransaction())
                .Returns(transactionMock.Object);

            var command = new Mock<Command.ICommand>();
            command.Setup(r => r.Execute(It.IsAny<IDatastoreConnectionProxy>()))
                .ReturnsAsync(1);

            //Start transaction
            _context.BeginTransaction();
            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            var result = await _context.Execute(command.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);

            _dbProviderMock.VerifyAll();
            _datastoreConnectionProxyFactoryMock.VerifyAll();

            dbConnectionMock.Setup(r => r.State).Returns(ConnectionState.Open);

            _context.Dispose();

            dbConnectionMock.VerifyAll();
            dbConnectionMock.Verify(r => r.Open(), Times.Once);
            transactionMock.VerifyAll();

            dbConnectionMock.Verify(r => r.Close(), Times.Once);
            transactionMock.Verify(r => r.Dispose(), Times.Once);
        }
    }
}