using Microsoft.AspNetCore.SignalR;

namespace TodoListAPI.SignalR.Infrastructure
{
    public interface IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection);
    }
}
