using Microsoft.Extensions.DependencyInjection;
using Videa.Datastore.Connection;
using Videa.Datastore.Extensions.Microsoft.DependencyInjection;

namespace Videa.Datastore.Dapper.Extensions.Microsoft.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVideaDatastoreDapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddVideaDatastore();
            serviceCollection.AddTransient(typeof(IDatastoreConnectionProxy), typeof(DapperDatastoreConnectionProxy));
            serviceCollection.AddTransient(typeof(IDatastoreConnectionProxyFactory), typeof(DapperDatastoreConnectionProxyFactory));

            return serviceCollection;
        }
    }
}
