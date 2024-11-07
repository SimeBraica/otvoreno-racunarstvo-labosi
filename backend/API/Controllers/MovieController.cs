using API.Models;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase {

        private MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filmovi>>> Movies() {
            var movies = await _movieService.GetMovies();
            return Ok(movies);
        }

        [HttpGet("MoviesByNaziv")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesByNaziv(string naziv) {
            var movies = await _movieService.GetFilteredMoviesByNaziv(naziv);
            return Ok(movies);
        }

        [HttpGet("MoviesByZanr")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesByZanr(string zanr) {
            var movies = await _movieService.GetFilteredMoviesByZanr(zanr);
            return Ok(movies);
        }

        [HttpGet("MoviesByWildcard")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesByWildcard(string wildcard) {
            var movies = await _movieService.GetFilteredMoviesByWildCard(wildcard);
            return Ok(movies);
        }
    }
}
