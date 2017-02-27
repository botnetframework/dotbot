using System.Threading;
using System.Threading.Tasks;

namespace Dotbot
{
    public interface IWorker
    {
        string FriendlyName { get; }
        Task<bool> Run(CancellationToken token);
    }
}
