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

    // Vi returnere en bool i denne, hvis vi skal opdatere status på lobbien. 
    // Altså hvis den for eksempel skal have skiftet state fra 2 spillere venter til en spiller venter. Så spilleren kan se at ham han spillede mod ikke længere er til stede.
    public Guid? RemoveConnection(string connectionId)
    {
        var id = _connections.FirstOrDefault(x => x.Value == connectionId).Key;
        _connections.Remove(_connections.FirstOrDefault(x => x.Value == connectionId).Key);
        foreach (Lobby lobby in _lobbies.ToList())
        {
            if (lobby.Members != null)
            {
                if (lobby.Members.Contains(id))
                {
                    lobby.Members.Remove(id);
                    if (lobby.Members.Count == 0)
                    {
                        _lobbies.Remove(lobby);
                        return null;
                    }
                    else
                    {
                        lobby.Status = GameStatus.OnePlayerWaiting;
                        return lobby.Id;
                    }
                }
            }
        }
        return null;
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

    public bool AddPlayerToLobby(Guid lobbyId, int userId)
    {
        var lobby = _lobbies?.FirstOrDefault(x => x.Id == lobbyId);
        if (lobby == null)
        {
            return false;
        }
        else if (lobby.Members.Count == 2)
        {
            return false;
        }
        else
        {
            lobby.Members.Add(userId);
            lobby.MemberReadyState[userId] = false;
            if (lobby.Members.Count == 2)
            {
                lobby.Status = GameStatus.TwoPlayersWaiting;
            }
            return true;
        }
    }

    public bool RemovePlayerFromLobby(Guid lobbyId, int userId)
    {
        var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId);
        if (lobby == null)
        {
            return false;
        }
        else if (lobby.Members.Count == 1)
        {
            _lobbies.Remove(lobby);
            return true;
        }
        else
        {
            lobby.Members.Remove(userId);
            lobby.Status = GameStatus.OnePlayerWaiting;
            return true;
        }
    }

    public Lobby? GetLobby(Guid? lobbyId)
    {
        var lobby = _lobbies.FirstOrDefault(x => x.Id == lobbyId);
        return lobby;
    }
    public GameStatus GetLobbyStatus(Guid lobbyId)
    {
        return _lobbies.FirstOrDefault(x => x.Id == lobbyId).Status;
    }

    public List<Lobby> GetLobbies()
    {
        return _lobbies;
    }

}