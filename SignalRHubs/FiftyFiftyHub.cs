using Microsoft.AspNetCore.SignalR;

namespace farmaatte_api.SignalRHubs;

public class FiftyFiftyHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", $"{Context.ConnectionId} has joined");
    }

    public async Task JoinGroup(string message){
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

}
