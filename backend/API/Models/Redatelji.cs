using System;
using System.Collections.Generic;
using API.Models;

namespace API;

public partial class Redatelji
{
    public int RedateljId { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public virtual ICollection<Filmovi> Filmovis { get; set; } = new List<Filmovi>();
}
