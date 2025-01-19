namespace DbToCsv {

    public record Film {
        public int Film_id { get; set; }
        public string Naziv { get; set; }
        public string Zemlja { get; set; }
        public int Prosjecna_Ocjena { get; set; }
        public int Godina { get; set; }
        public int Trajanje { get; set; }
        public string Kratki_opis { get; set; }
        public int Budzet { get; set; }
        public int Prihod { get; set; }
        public string Ime_distributera { get; set; }
        public string TVPG_ocjena { get; set; }
        public List<Glumac> Glumci { get; set; }
        public string Redatelj_ime { get; set; }
        public string Redatelj_prezime { get; set; }
        public List<Zanr> Zanrovi { get; set; }
    }
}