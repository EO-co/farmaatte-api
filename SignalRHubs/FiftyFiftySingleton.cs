using farmaatte_api.Models;

namespace farmaatte_api.SignalRHubs;

public class FiftyFiftySingleton()
{
    private static FiftyFiftySingleton? _instance;
    private static readonly object _lock = new object();
    private Dictionary<int, string> _connections = new Dictionary<int, string>();
    private List<Lobby> _lobbies = new List<Lobby>();

    public static FiftyFiftySingleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new FiftyFiftySingleton();
                }
                return _instance;
            }
        }
    }

    public void AddConnection(int userId, string connectionId)
    {
        if (_connections.ContainsKey(userId))
        {
            _connections[userId] = connectionId;
            return;
        }
        else
        {
            _connections.Add(userId, connectionId);
        }
    }

    public string GetConnection(int userId)
    {
        return _connections[userId];
    }

    public Dictionary<int, string> GetAllConnections()
    {
        return _connections;
    }

    public void RemoveConnection(int userId)
    {
        _connections.Remove(userId);
    }

    public void RemoveConnection(string connectionIdId)
    {
        _connections.Remove(_connections.FirstOrDefault(x => x.Value == connectionIdId).Key);
    }

    public Guid CreateLobby(int userId)
    {
        var lobby = new Lobby
        {
            Id = Guid.NewGuid()
        };
        lobby.Members.Add(userId);
        lobby.Status = GameStatus.OnePlayerWaiting;
        _lobbies.Add(lobby);
        lobby.MemberReadyState[userId] = false;
        return lobby.Id;
    }

    public void RemoveLobby(Guid lobbyId)
    {
        _lobbies.Remove(_lobbies.FirstOrDefault(x => x.Id == lobbyId));
    }

    public bool AddPlayerToLobby(Guid lobbyId, int userId) {
        var lobby = _lobbies?.FirstOrDefault(x => x.Id == lobbyId);
        if (lobby == null)
        {
            return false;
        }
        else if (lobby.Members.Count == 2)
        {
            return false;
        } else {
            lobby.Members.Add(userId);
            lobby.MemberReadyState[userId] = false;
            if (lobby.Members.Count == 2)
            {
                lobby.Status = GameStatus.TwoPlayersWaiting;
            }
            return true;
        }
    }

    public bool RemovePlayerFromLobby(Guid lobbyId, int userId) {
        var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId);
        if (lobby == null)
        {
            return false;
        } else if(lobby.Members.Count == 1) {
            _lobbies.Remove(lobby);
            return true;
        } else {
            lobby.Members.Remove(userId);
            lobby.Status = GameStatus.OnePlayerWaiting;
            return true;
        }
    }
    public GameStatus GetLobbyStatus(Guid lobbyId) {
        return _lobbies.FirstOrDefault(x => x.Id == lobbyId).Status;
    }

    public List<Lobby> GetLobbies() {
        return _lobbies;
    }

}