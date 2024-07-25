using System;
using System.Collections.Generic;

namespace farmaatte_api.Models;

public partial class Profilepicture
{
    public int Profilepictureid { get; set; }

    public int Userid { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
