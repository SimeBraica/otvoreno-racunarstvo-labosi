using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Filmovi
{
    public int FilmId { get; set; }

    public string Naziv { get; set; } = null!;

    public string? Zemlja { get; set; }

    public int? ProsjecnaOcjena { get; set; }

    public int? Godina { get; set; }

    public int? Trajanje { get; set; }

    public string? KratkiOpis { get; set; }

    public long? Budzet { get; set; }

    public long? Prihod { get; set; }

    public int? RedateljId { get; set; }

    public string? ImeDistributera { get; set; }

    public string? TvpgOcjena { get; set; }

    public virtual Redatelji? Redatelj { get; set; }

    public virtual ICollection<Glumci> Glumacs { get; set; } = new List<Glumci>();

    public virtual ICollection<Zanrovi> Zanrs { get; set; } = new List<Zanrovi>();
}
