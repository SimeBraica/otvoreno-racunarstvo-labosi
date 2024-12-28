using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Net.Sockets;
using API.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;


namespace API.Repositories {
    public class MovieRepository : Repository<Filmovi> {

        private readonly IMapper _mapper;
        public MovieRepository(MoviesDbContext context, IMapper mapper) : base(context) {
            _mapper = mapper;
        }
        public async Task<IEnumerable<Filmovi>> Get() {
            return await Context.Filmovis
                .Include(g => g.Glumacs)
                .Include(z => z.Zanrs)
                .Include(r => r.Redatelj)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Filmovi>> GetById(int id) {
            return await Context.Filmovis
                .Include(g => g.Glumacs)
                .Include(z => z.Zanrs)
                .Include(r => r.Redatelj)
                .AsNoTracking()
                .Where(m => m.FilmId == id)
                .ToListAsync();
        }

        public async Task<Filmovi> CreateMovie(PostMovieDTO movie) {
            Filmovi _film = null;
            _film = new Filmovi {
                Naziv = movie.Naziv,
                Zemlja = movie.Zemlja,
                ProsjecnaOcjena = movie.ProsjecnaOcjena,
                Godina = movie.Godina,
                Trajanje = movie.Trajanje,
                KratkiOpis = movie.KratkiOpis,
                Budzet = movie.Budzet,
                Prihod = movie.Prihod,
                RedateljId = null,
                Redatelj = null,
                ImeDistributera = movie.ImeDistributera,
                TvpgOcjena = movie.TVPGocjena,
                Glumacs = new List<Glumci>(),
                Zanrs = new List<Zanrovi>()
            };
            Context.Add(_film);
            await Context.SaveChangesAsync();

            return _film;
        }

        public async Task<bool> RemoveMovie(int id) {
            var _movie = await Context.Filmovis.FirstOrDefaultAsync(a => a.FilmId == id);
            if(_movie == null) {
                return false;
            }
            Context.Filmovis.Remove(_movie);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMovie(MovieDTO movie) {
            var _movie = await Context.Filmovis.FirstOrDefaultAsync(a => a.FilmId == movie.FilmId);
            if (_movie == null) {
                return false;
            }
            _mapper.Map(movie, _movie);
            await Context.SaveChangesAsync();
            return true;

        }

    }
}

