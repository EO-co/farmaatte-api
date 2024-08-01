
using farmaatte_api.Models;

namespace farmaatte_api.DTOs;

public class LobbyOverviewDTO
{
    public Guid Id { get; set; }
    public int NumberOfPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public GameStatus Status { get; set; }
}