

namespace farmaatte_api.DTOs;

public class ProfileDTO
{
    public required string Name { get; set; }
    public string? Nickname { get; set; }
    public byte[]? ProfilePicture { get; set; }

}