namespace UI.Models {
    public class MovieWithoutExpandedDTO {
        public int FilmId { get; set; }
        public string Naziv { get; set; }
        public string Zemlja { get; set; }
        public int? ProsjecnaOcjena { get; set; }
        public int? Godina { get; set; }
        public int? Trajanje { get; set; }
        public string KratkiOpis { get; set; }
        public long? Budzet { get; set; }
        public long? Prihod { get; set; }
        public string RedateljIme { get; set; }
        public string RedateljPrezime { get; set; }
        public string ImeDistributera { get; set; }
        public string TVPGocjena { get; set; }

        public List<GlumacDTO> Glumci { get; set; }
        public List<ZanrDTO> Zanrovi { get; set; }
    }
}
