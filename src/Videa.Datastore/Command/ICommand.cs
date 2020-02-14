using System.Threading.Tasks;
using Videa.Datastore.Connection;

namespace Videa.Datastore.Command
{
    public interface ICommand
    {
        Task<int> Execute(IDatastoreConnectionProxy datastoreConnectionProxy);
    }
}