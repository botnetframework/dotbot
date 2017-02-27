using System.Threading.Tasks;
using Dotbot.Models;

namespace Dotbot
{
    public interface IBroker
    {
        Task Reply(Room room, User fromUser, string text);

        Task Broadcast(Room room, string text);
    }
}
