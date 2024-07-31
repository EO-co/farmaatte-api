
namespace farmaatte_api.DTOs;

public class ProfileOverviewDTO
{
    public int Id {get; set;}
    public string Name { get; set; }
    public string? Nickname { get; set; }
    public byte[]? Picture { get; set; }

}