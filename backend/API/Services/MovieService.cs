using API.Models;
using API.Models.DTO;
using API.Repositories;

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
                    RedateljIme = movie.Redatelj.Ime,
                    RedateljPrezime = movie.Redatelj.Prezime,
                    ImeDistributera = movie.ImeDistributera,
                    TVPGocjena = movie.TvpgOcjena,
                    Glumci = GetAllGlumac(movie),
                    Zanrovi = GetAllZanr(movie)

                };
                formatedMovies.Add(formatedMovie);
            }
            return formatedMovies;
        }

        private List<GlumacDTO> GetAllGlumac(Filmovi film) {
            List<GlumacDTO> glumacList = new();
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