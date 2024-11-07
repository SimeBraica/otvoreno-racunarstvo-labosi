using System;
using System.Collections.Generic;
using API.Models;

namespace API;

public partial class Zanrovi
{
    public int ZanrId { get; set; }

    public string Ime { get; set; } = null!;

    public virtual ICollection<Filmovi> Films { get; set; } = new List<Filmovi>();
}
