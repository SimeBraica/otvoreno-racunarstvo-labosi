using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace API.Repositories {
    public class MovieRepository : Repository<Filmovi> {
        public MovieRepository(MoviesDbContext context) : base(context) {
        }
        public async Task<IEnumerable<Filmovi>> Get() {
            return await Context.Filmovis
                .Include(g => g.Glumacs)
                .Include(z => z.Zanrs)
                .Include(r => r.Redatelj)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

