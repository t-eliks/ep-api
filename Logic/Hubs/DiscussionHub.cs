using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Logic.Hubs
{
    public class DiscussionHub : Hub
    {
        public async Task JoinGroup(string projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
        }

        public async Task LeaveGroup(string projectId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId);
        }
    }
}
