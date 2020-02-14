using System.Data;

namespace Videa.Datastore.Provider
{
    public interface IDbProvider
    {
        IDbConnection GetConnection();
    }
}