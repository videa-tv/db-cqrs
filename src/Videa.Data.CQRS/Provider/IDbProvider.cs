using System.Data;

namespace Videa.Data.CQRS.Provider
{
    public interface IDbProvider
    {
        IDbConnection GetConnection();
    }
}