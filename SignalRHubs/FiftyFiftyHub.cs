using farmaatte_api.DTOs;
using farmaatte_api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.VisualBasic;
using Newtonsoft.Json;


namespace farmaatte_api.SignalRHubs;

public class FiftyFiftyHub : Hub
{

    private readonly FarmaatteDbContext _context;
    public FiftyFiftyHub(FarmaatteDbContext context)
    {
        _context = context;
    }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("User connected: " + Context.ConnectionId);
        await Clients.All.SendAsync("UserConnected", $"{Context.ConnectionId} has joined");
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine("User disconnected: " + Context.ConnectionId);
        var _instance = FiftyFiftySingleton.Instance;
        Guid? lobbyId = _instance.RemoveConnection(Context.ConnectionId);
        if (lobbyId != null)
        {
            await UpdateLobbyStatus(lobbyId);
        }
        await Clients.All.SendAsync("UserDisconnected", $"{Context.ConnectionId} has left");
        await SendLobbyOverview();
    }

    public async Task UpdateLobbyStatus(Guid? lobbyId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        var lobby = Instance.GetLobby(lobbyId);
        if (lobby != null)
        {
            await Clients.Groups(lobbyId.ToString()).SendAsync("LobbyStatus", lobby);
        }
    }



    public async Task IdentifyUser(int Id)
    {
        Console.WriteLine("Joining group: " + Id);
        var _instance = FiftyFiftySingleton.Instance;
        _instance.AddConnection(Id, Context.ConnectionId);
        var connections = _instance.GetAllConnections();
        Console.WriteLine("Connections");
        foreach (var connection in connections)
        {
            Console.WriteLine(connection.Key + " " + connection.Value);
        }
        var Lobbies = _instance.GetLobbies();
        await Clients.Caller.SendAsync("ReceiveOverview", Lobbies);
    }

    public async Task CreateLobby(int UserId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        var lobbyId = Instance.CreateLobby(UserId);
        await AddUserToGroup(Context.ConnectionId, lobbyId);
        Console.WriteLine("User " + UserId + " created lobby " + lobbyId);
        await SendLobbyOverview();
    }

    public async void JoinLobby(Guid LobbyId, int UserId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        if (Instance.AddPlayerToLobby(LobbyId, UserId))
        {
            await AddUserToGroup(Context.ConnectionId, LobbyId);
            Console.WriteLine("User " + UserId + " joined lobby " + LobbyId);
            await SendLobbyOverview();
        }
        else
        {
            await Clients.Caller.SendAsync("LobbyFull", "Lobby is full");
        }
    }

    public async Task AddUserToGroup(string ConnectionId, Guid LobbyId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        var Lobby = Instance.GetLobbies().FirstOrDefault(x => x.Id == LobbyId);
        await Groups.AddToGroupAsync(ConnectionId, LobbyId.ToString());
        await Clients.Group(LobbyId.ToString()).SendAsync("LobbyStatus", Lobby);
        Console.WriteLine(ConnectionId + " added to group: " + LobbyId.ToString());
    }

    public async Task SendLobbyOverview()
    {
        var Instance = FiftyFiftySingleton.Instance;
        var Lobbies = Instance.GetLobbies();
        await Clients.All.SendAsync("ReceiveOverview", Lobbies);
    }

    public async void LeaveLobby(Guid LobbyId, int UserId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        if (Instance.RemovePlayerFromLobby(LobbyId, UserId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, LobbyId.ToString());
            Console.WriteLine("User " + UserId + " left lobby " + LobbyId);
            await SendLobbyOverview();
        }
        else
        {
            await Clients.Caller.SendAsync("LobbyEmpty", "Lobby is empty");
        }
    }

    public async void MemberReady(Guid LobbyId, int UserId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        var lobby = Instance.GetLobbies().FirstOrDefault(x => x.Id == LobbyId);
        if (lobby != null)
        {
            if (lobby.MemberReadyState != null)
            {
                lobby.MemberReadyState[UserId] = true;
                if (lobby.MemberReadyState.All(x => x.Value))
                {
                    lobby.Status = GameStatus.CountStarted;
                    await Clients.Group(LobbyId.ToString()).SendAsync("LobbyStatus", lobby);
                }
            }
        }
    }

    public async void MemberUnReady(Guid LobbyId, int UserId)
    {
        var Instance = FiftyFiftySingleton.Instance;
        var lobby = Instance.GetLobbies().FirstOrDefault(x => x.Id == LobbyId);
        if (lobby != null)
        {
            if (lobby.MemberReadyState != null)
            {
                lobby.MemberReadyState[UserId] = false;
                lobby.Status = GameStatus.TwoPlayersWaiting;
                lobby.SetResult();
                await Clients.Group(LobbyId.ToString()).SendAsync("LobbyStatus", lobby);
            }
        }
    }




}
