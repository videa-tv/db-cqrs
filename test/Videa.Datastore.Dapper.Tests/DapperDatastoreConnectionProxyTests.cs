using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Videa.Datastore.Dapper.Tests
{
    [TestClass]
    public class DapperDatastoreConnectionProxyTests
    {
        private Mock<IDbConnection> _dbConnectionMock;
        private Mock<IDbTransaction> _dbTransactionMock;

        private DapperDatastoreConnectionProxy _dapperDatastoreConnectionProxy;

        [TestInitialize]
        public void Initialize()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _dbTransactionMock = new Mock<IDbTransaction>();

            _dapperDatastoreConnectionProxy = new DapperDatastoreConnectionProxy(_dbConnectionMock.Object, _dbTransactionMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DapperDatastoreConnectionProxy_Constructor_Null_Connection()
        {
            try
            {
                _dapperDatastoreConnectionProxy = new DapperDatastoreConnectionProxy(null, _dbTransactionMock.Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "dbConnection");
                throw;
            }
        }
    }
}
