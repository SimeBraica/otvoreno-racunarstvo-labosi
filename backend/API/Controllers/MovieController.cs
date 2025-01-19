using API.Models;
using API.Models.DTO;
using API.Models.ResponseWrapper;
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
        public async Task<ActionResult<ResponseWrapper<List<MovieDTO>>>> Movies() {
            try {
                var movies = await _movieService.GetMovies();
                if (movies == null) {
                    return NotFound(ResponseWrapperMethods<MovieDTO>.ReturnNotFound404());
                }
                return Ok(ResponseWrapperMethods<List<MovieDTO>>.ReturnSuccess200(movies));
            } catch(Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }
            
        }
        
        [HttpGet("MoviesByNaziv")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesByNaziv(string? naziv) {
            try {
                if (string.IsNullOrWhiteSpace(naziv)) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                var movies = await _movieService.GetFilteredMoviesByNaziv(naziv); 
                if (movies.Count == 0 || movies == null) {
                    return StatusCode(404, (ResponseWrapperMethods<MovieDTO>.ReturnNotFound404()));
                }
                return Ok(ResponseWrapperMethods<List<MovieDTO>>.ReturnSuccess200(movies));
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }

        }

        [HttpGet("MoviesByZanr")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesByZanr(string? zanr) {
            try {
                if (string.IsNullOrWhiteSpace(zanr) || zanr.GetType() != typeof(string)) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                var movies = await _movieService.GetFilteredMoviesByZanr(zanr);
                if (movies.Count == 0 || movies == null) {
                    return StatusCode(404, (ResponseWrapperMethods<MovieDTO>.ReturnNotFound404()));
                }
                return Ok(ResponseWrapperMethods<List<MovieDTO>>.ReturnSuccess200(movies));
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }
        }

        [HttpGet("MoviesByWildcard")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesByWildcard(string? wildcard) {
            try {
                if (string.IsNullOrWhiteSpace(wildcard)) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                var movies = await _movieService.GetFilteredMoviesByWildCard(wildcard);
                if (movies.Count == 0 || movies == null) {
                    return StatusCode(404, (ResponseWrapperMethods<MovieDTO>.ReturnNotFound404()));
                }
                return Ok(ResponseWrapperMethods<List<MovieDTO>>.ReturnSuccess200(movies));
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }
        }

        [HttpGet("MoviesById")]
        public async Task<ActionResult<IEnumerable<Filmovi>>> MoviesById(int? id) {
            try {
                if (string.IsNullOrWhiteSpace(id.ToString())) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                var movies = await _movieService.GetMoviesById((int)id);
                if (movies == null || movies.FilmId == 0) {
                    return StatusCode(200, (ResponseWrapperMethods<MovieDTO>.ReturnSuccess200(null)));
                }
                return Ok(ResponseWrapperMethods<MovieDTO>.ReturnSuccess200(movies));
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }
        }

        [HttpPost("CreateMovie")]
        public async Task<IActionResult> PostMovie([FromBody] PostMovieDTO? movie) {
            if (!ModelState.IsValid) {
                return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
            }
            try {
                var movies = await _movieService.AddMovie(movie);
                if (!movies) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                return StatusCode(201, ResponseWrapperMethods<MovieDTO>.Created201());
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }
        }

        [HttpDelete("DeleteMovie")]
        public async Task<IActionResult> DeleteMovie(int id) {
            try {
                var movies = await _movieService.DeleteMovie(id);
                if (!movies) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                return StatusCode(200, (ResponseWrapperMethods<List<MovieDTO>>.ReturnSuccess200(null)));
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());
            }
        }

        [HttpPut("UpdateMovie")]
        public async Task<IActionResult> PutMovie([FromBody] MovieDTO movie) {
            if (!ModelState.IsValid) {
                return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
            }
            try {
                var movies = await _movieService.UpdateMovie(movie);
                if (!movies) {
                    return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
                }
                return StatusCode(200, (ResponseWrapperMethods<List<MovieDTO>>.ReturnSuccess200(null)));
            } catch (Exception ex) {
                return StatusCode(500, ResponseWrapperMethods<MovieDTO>.ReturnServerError500());

            }
        }

        [HttpGet("GetJsonLd")]
        public async Task<IActionResult> GetMovieAsJsonLd(string name) {
            if (string.IsNullOrEmpty(name)) {
              return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
            }
            var movie = await _movieService.GetFilteredMoviesByNaziv(name);
            if(movie == null) {
                return BadRequest(ResponseWrapperMethods<MovieDTO>.BadRequest400());
            }
            var jsonLd = new {
                @context = "https://schema.org",
                @type = "Movie",
                name = movie[0].Naziv,
                countryOfOrigin = movie[0].Zemlja,
                aggregateRating = new {
                    @type = "AggregateRating",
                    ratingValue = movie[0].ProsjecnaOcjena,
                    ratingCount = 1
                },
                datePublished = movie[0].Godina,
                duration = $"PT{movie[0].Trajanje / 60}H{movie[0].Trajanje % 60}M",
                description = movie[0].KratkiOpis,
                director = new {
                    @type = "Person",
                    givenName = movie[0].RedateljIme,
                    familyName = movie[0].RedateljPrezime
                },
                productionCompany = new {
                    @type = "Organization",
                    name = movie[0].ImeDistributera
                },
                contentRating = movie[0].TVPGocjena,
                actor = movie[0].Glumci.Select(a => new {
                    @type = "Person",
                    givenName = a.Ime,
                    familyName = a.Prezime
                }),
                genre = movie[0].Zanrovi.Select(a => a.Ime).ToArray(),
                budget = new {
                    @type = "MonetaryAmount",
                    value = movie[0].Budzet,
                    currency = "USD"
                },
                boxOffice = new {
                    @type = "MonetaryAmount",
                    value = movie[0].Prihod,
                    currency = "USD"
                }
            };

            return Ok(jsonLd);
        }


    }
}
