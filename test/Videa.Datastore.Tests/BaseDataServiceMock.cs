using Videa.Datastore.Provider;

namespace Videa.Datastore.Tests
{
    public class BaseDataServiceMock : BaseDataService<IDbProvider>
    {
        public BaseDataServiceMock(IDatastoreContext<IDbProvider> datastoreContext) : base(datastoreContext)
        {
        }
    }
}