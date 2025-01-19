using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Glumci
{
    public int GlumacId { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public virtual ICollection<Filmovi> Films { get; set; } = new List<Filmovi>();
}
