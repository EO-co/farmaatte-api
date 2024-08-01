
namespace farmaatte_api.Models; 

public enum GameStatus {
    OnePlayerWaiting, 
    TwoPlayersWaiting,
    CountStarted,
    GameFinished
}

public class Lobby {

    public Guid Id { get; set; }
    public int MaxPlayers { get; } = 2;
    public List<int>? Members { get; set; } = new List<int>();
    public GameStatus Status { get; set; }
    public Dictionary<int, bool>? MemberReadyState {get; set;} = new Dictionary<int, bool>();

    public int Result { get; set; }

    public void SetResult() {
        Result = new Random().Next(0, 2);
    }


}