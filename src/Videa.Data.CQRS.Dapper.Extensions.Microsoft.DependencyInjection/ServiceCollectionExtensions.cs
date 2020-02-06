using Microsoft.Extensions.DependencyInjection;
using Videa.Data.CQRS.Connection;
using Videa.Data.CQRS.Extensions.Microsoft.DependencyInjection;

namespace Videa.Data.CQRS.Dapper.Extensions.Microsoft.DependencyInjection
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
