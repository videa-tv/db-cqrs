using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Datastore.Provider;

namespace Videa.Datastore.Tests
{
    [TestClass]
    public class BaseDataServiceTests
    {
        private BaseDataService<IDbProvider> _baseDataService;

        [TestMethod]
        public void Constructor_BaseDataService()
        {
            var datastoreContextFactory = new Mock<IDatastoreContext<IDbProvider>>();

            _baseDataService = new BaseDataServiceMock(datastoreContextFactory.Object);

            Assert.IsNotNull(_baseDataService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_BaseDataService_Null()
        {
            try
            {
                _baseDataService = new BaseDataServiceMock(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.ParamName == "datastoreContext");
                throw;
            }
        }
    }
}
