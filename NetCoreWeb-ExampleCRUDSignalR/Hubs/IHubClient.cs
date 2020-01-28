using System.Threading.Tasks;

namespace NetCoreWeb_ExampleCRUDSignalR.Hubs
{
    public interface IHubClient
    {
        Task BroadcastMessage();
    }
}
