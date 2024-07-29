

namespace farmaatte_api.DTOs;


public class LoginSucceededDTO
{

    public string Token { get; set; }
    public long Expires { get; set; }
    public int Id { get; set; }
    public LoginSucceededDTO() { }

    public LoginSucceededDTO(string token, long expires, int id)
    {
        Token = token;
        Expires = expires;
        Id = id;
    }
}