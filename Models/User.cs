using System;
using System.Collections.Generic;

namespace farmaatte_api.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Pwhash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Nickname { get; set; }

    public int Groupid { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<Profilepicture> Profilepictures { get; set; } = new List<Profilepicture>();

    public virtual ICollection<Result> ResultPlayer1s { get; set; } = new List<Result>();

    public virtual ICollection<Result> ResultPlayer2s { get; set; } = new List<Result>();

    public virtual ICollection<Result> ResultWinnerNavigations { get; set; } = new List<Result>();
}
