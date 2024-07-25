using System;
using System.Collections.Generic;

namespace farmaatte_api.Models;

public partial class Result
{
    public int Resultid { get; set; }

    public int Gameid { get; set; }

    public int Player1Id { get; set; }

    public int Player2Id { get; set; }

    public int Winner { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User Player1 { get; set; } = null!;

    public virtual User Player2 { get; set; } = null!;

    public virtual User WinnerNavigation { get; set; } = null!;
}
