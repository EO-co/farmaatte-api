using System;
using System.Collections.Generic;

namespace farmaatte_api.Models;

public partial class Grouppicture
{
    public int Grouppictureid { get; set; }

    public int Groupid { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}
