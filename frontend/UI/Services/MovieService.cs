using UI.Models;
namespace UI.Services {
    public class MovieService {
        public async Task<string> DownloadCsv(List<MovieDTO> movies) {
            var csvBuilder = new System.Text.StringBuilder();
            csvBuilder.AppendLine("Film ID,Naziv,Zemlja,Prosjecna Ocjena,Godina,Trajanje,Kratki Opis,Budzet,Prihod,Distributer,TVPG Ocjena,Redatelj Ime,Redatelj Prezime,Glumac Ime, Glumac Prezime, Zanr");
            foreach (var movie in movies) {
                foreach (var zanr in movie.Zanrovi) {
                    foreach (var glumac in movie.Glumci) {
                        csvBuilder.AppendLine($"{movie.FilmId},{movie.Naziv},{movie.Zemlja},{movie.ProsjecnaOcjena},{movie.Godina},{movie.Trajanje},{movie.KratkiOpis},{movie.Budzet},{movie.Prihod},{movie.ImeDistributera},{movie.TVPGocjena},{movie.RedateljIme} {movie.RedateljPrezime},{glumac.Ime}, {glumac.Prezime},{zanr.Ime}");
                    }
                }
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(csvBuilder.ToString());
            var dataUri = "data:text/csv;charset=utf-8," + Uri.EscapeDataString(csvBuilder.ToString());
            return dataUri;
        }

        public string GetGlumci(List<GlumacDTO> glumciMovie) {
            string glumci = "";
            foreach (var glumac in glumciMovie) {
                glumci += glumac.Ime + " " + glumac.Prezime + " ";
            }
            return glumci;
        }

        public string GetZanrovi(List<ZanrDTO> zanrMovie) {
            string zanrovi = "";
            foreach (var zanr in zanrMovie) {
                zanrovi += zanr.Ime + " ";
            }
            return zanrovi;
        }

        public List<MovieWithoutExpandedDTO> FormatJsonFromList(List<MovieDTO> movies) {
            List<MovieWithoutExpandedDTO> formattedMovies = new();
            foreach (var movie in movies) {
                formattedMovies.Add(new MovieWithoutExpandedDTO {
                    FilmId = movie.FilmId,
                    Naziv = movie.Naziv,
                    Zemlja = movie.Zemlja,
                    ProsjecnaOcjena = movie.ProsjecnaOcjena,
                    Godina = movie.Godina,
                    Trajanje = movie.Trajanje,
                    KratkiOpis = movie.KratkiOpis,
                    Budzet = movie.Budzet,
                    Prihod = movie.Prihod,
                    RedateljIme = movie.RedateljIme,
                    RedateljPrezime = movie.RedateljPrezime,
                    ImeDistributera = movie.ImeDistributera,
                    TVPGocjena = movie.TVPGocjena,
                    Glumci = movie.Glumci,
                    Zanrovi = movie.Zanrovi
                });
            }
            return formattedMovies;
        }
    }
}
