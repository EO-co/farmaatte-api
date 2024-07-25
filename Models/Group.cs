using System;
using System.Collections.Generic;

namespace farmaatte_api.Models;

public partial class Group
{
    public int Groupid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Grouppicture> Grouppictures { get; set; } = new List<Grouppicture>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
