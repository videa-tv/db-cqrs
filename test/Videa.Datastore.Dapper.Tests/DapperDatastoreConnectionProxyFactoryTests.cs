using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Videa.Datastore.Dapper.Tests
{
    [TestClass]
    public class DapperDatastoreConnectionProxyFactoryTests
    {
        private DapperDatastoreConnectionProxyFactory _dapperDatastoreConnectionProxyFactory;

        [TestInitialize]
        public void Initialize()
        {
            _dapperDatastoreConnectionProxyFactory = new DapperDatastoreConnectionProxyFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DapperDatastoreConnectionProxyFactory_Create_NullConnection()
        {
            _dapperDatastoreConnectionProxyFactory.Create(null, null);
        }

        [TestMethod]
        public void DapperDatastoreConnectionProxyFactory_Create_NullTransaction()
        {
            var dbConnectionMock = new Mock<IDbConnection>();

            var result = _dapperDatastoreConnectionProxyFactory.Create(dbConnectionMock.Object, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(DapperDatastoreConnectionProxy), result.GetType());
        }

        [TestMethod]
        public void DapperDatastoreConnectionProxyFactory_Create()
        {
            var dbConnectionMock = new Mock<IDbConnection>();
            var dbTransactionMock = new Mock<IDbTransaction>();

            var result = _dapperDatastoreConnectionProxyFactory.Create(dbConnectionMock.Object, dbTransactionMock.Object);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(DapperDatastoreConnectionProxy), result.GetType());
        }
    }
}
