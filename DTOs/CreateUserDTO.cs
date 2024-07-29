

namespace farmaatte_api.DTOs;

public class CreateUserDTO
{

    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Nickname { get; set; }
    public int Groupid { get; set; }

    public CreateUserDTO() { }

}