using System.Threading.Tasks;
using Videa.Data.CQRS.Connection;

namespace Videa.Data.CQRS.Command
{
    public interface ICommand
    {
        Task<int> Execute(IDatastoreConnectionProxy datastoreConnectionProxy);
    }
}