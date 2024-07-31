
namespace farmaatte_api.DTOs;

public class ProfileOverviewDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Nickname { get; set; }
    public byte[]? Picture { get; set; }

}