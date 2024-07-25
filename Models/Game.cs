using System;
using System.Collections.Generic;

namespace farmaatte_api.Models;

public partial class Game
{
    public int Gameid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
