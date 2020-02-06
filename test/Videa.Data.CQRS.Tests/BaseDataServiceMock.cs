using Videa.Data.CQRS.Provider;

namespace Videa.Data.CQRS.Tests
{
    public class BaseDataServiceMock : BaseDataService<IDbProvider>
    {
        public BaseDataServiceMock(IDatastoreContext<IDbProvider> datastoreContext) : base(datastoreContext)
        {
        }
    }
}