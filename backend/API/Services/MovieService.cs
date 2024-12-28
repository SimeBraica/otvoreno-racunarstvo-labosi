using API.Models;
using API.Models.DTO;
using API.Repositories;
using System.Formats.Asn1;

namespace API.Services {
    public class MovieService {

        private MovieRepository _movieRepository;
        public MovieService(MovieRepository movieRepository) {
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieDTO>> GetFilteredMoviesByNaziv(string naziv) {
            var movies = await GetMovies();
            return movies.Where(m => m.Naziv.ToLower().Contains(naziv.ToLower())).ToList();
        }
        public async Task<List<MovieDTO>> GetFilteredMoviesByZanr(string zanr) {
            var movies = await GetMovies();
            return movies.FindAll(z => z.Zanrovi.Any(i => i.Ime.ToLower().Contains(zanr.ToLower()))).ToList();
        }

        public async Task<List<MovieDTO>> GetFilteredMoviesByWildCard(string wildcard) {
            var movies = await GetMovies();
            return movies.Where(movie =>
                movie.GetType().GetProperties()
                    .Any(prop =>
                        prop.GetValue(movie).ToString().ToLower().Contains(wildcard.ToLower())
                    )
            ).ToList();
        }

        public async Task<bool> AddMovie(PostMovieDTO film) {
            if (film is not PostMovieDTO) {
                return false;
            }
            if (film.Budzet.ToString() == "" || film.Godina.ToString() == "" || film.ImeDistributera == "" ||
                film.KratkiOpis == "" || film.Naziv == "" || film.Prihod.ToString() == "" || film.ProsjecnaOcjena.ToString() == ""
                || film.Trajanje.ToString() == "" || film.TVPGocjena == "" || film.Zemlja == "") {
                return false;
            }
            if ((film.Budzet.GetType() != typeof(long)) || film.Godina.GetType() != typeof(int) || film.ImeDistributera.GetType() != typeof(string) || film.KratkiOpis.GetType() != typeof(string) || film.Naziv.GetType() != typeof(string) || film.Prihod.GetType() != typeof(long) || film.ProsjecnaOcjena.GetType() != typeof(int) || film.Trajanje.GetType() != typeof(int) || film.TVPGocjena.GetType() != typeof(string) || film.Zemlja.GetType() != typeof(string)) {
                return false;
            }
            var _film = await _movieRepository.CreateMovie(film);
            return true;
        }

        public async Task<bool> DeleteMovie(int id) {
            return await _movieRepository.RemoveMovie(id);
        }


        public async Task<bool> UpdateMovie(MovieDTO movie) {
            return await _movieRepository.UpdateMovie(movie);
        }


        public async Task<List<MovieDTO>> GetMovies() {
            var movies = await _movieRepository.Get();

            List<MovieDTO> formatedMovies = new();

            foreach (var movie in movies) {
                var formatedMovie = new MovieDTO {
                    FilmId = movie.FilmId,
                    Naziv = movie.Naziv,
                    Zemlja = movie.Zemlja,
                    ProsjecnaOcjena = movie.ProsjecnaOcjena,
                    Godina = movie.Godina,
                    Trajanje = movie.Trajanje,
                    KratkiOpis = movie.KratkiOpis,
                    Budzet = movie.Budzet,
                    Prihod = movie.Prihod,
                    RedateljIme = GetRedateljImePrezime(movie)[0],
                    RedateljPrezime = GetRedateljImePrezime(movie)[1],
                    ImeDistributera = movie.ImeDistributera,
                    TVPGocjena = movie.TvpgOcjena,
                    Glumci = GetAllGlumac(movie),
                    Zanrovi = GetAllZanr(movie)

                };
                formatedMovies.Add(formatedMovie);
            }
            return formatedMovies;
        }

        public async Task<MovieDTO> GetMoviesById(int id) {
            var movies = await _movieRepository.GetById(id);

            MovieDTO returnMovie = new();

            foreach (var movie in movies) {
                var formatedMovie = new MovieDTO {
                    FilmId = movie.FilmId,
                    Naziv = movie.Naziv,
                    Zemlja = movie.Zemlja,
                    ProsjecnaOcjena = movie.ProsjecnaOcjena,
                    Godina = movie.Godina,
                    Trajanje = movie.Trajanje,
                    KratkiOpis = movie.KratkiOpis,
                    Budzet = movie.Budzet,
                    Prihod = movie.Prihod,
                    RedateljIme = GetRedateljImePrezime(movie)[0],
                    RedateljPrezime = GetRedateljImePrezime(movie)[1],
                    ImeDistributera = movie.ImeDistributera,
                    TVPGocjena = movie.TvpgOcjena,
                    Glumci = GetAllGlumac(movie),
                    Zanrovi = GetAllZanr(movie)

                };
                returnMovie = formatedMovie;
            }
            return returnMovie;
        }

        private string[] GetRedateljImePrezime(Filmovi film) {
            return new string[]
            {
                film.Redatelj?.Ime ?? string.Empty,
                film.Redatelj?.Prezime ?? string.Empty
            };
        }

        private List<GlumacDTO> GetAllGlumac(Filmovi film) {
            List<GlumacDTO> glumacList = new();

            if (film.Glumacs == null) {
                return null;
            }
            foreach (var glumac in film.Glumacs) {
                var formattedGlumac = new GlumacDTO {
                    Ime = glumac.Ime,
                    Prezime = glumac.Prezime
                };
                glumacList.Add(formattedGlumac);
            }
            return glumacList;
        }

        private List<ZanrDTO> GetAllZanr(Filmovi film) {
            List<ZanrDTO> zanrList = new();
            if (film.Zanrs == null) {
                return null;
            }

            foreach (var zanr in film.Zanrs) {
                var formattedZanr = new ZanrDTO {
                    Ime = zanr.Ime
                };
                zanrList.Add(formattedZanr);
            }
            return zanrList;
        }
    }
}