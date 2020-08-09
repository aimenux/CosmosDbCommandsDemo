using System.Threading;
using System.Threading.Tasks;

namespace App.Commands
{
    public interface ICommand
    {
        Task<int> ExecuteAsync(CancellationToken cancellationToken);
    }
}
