using Microsoft.EntityFrameworkCore;

namespace API.Repositories {
    public abstract class Repository<T> : IDisposable where T : class {
        public MoviesDbContext Context { get; set; }
        public DbSet<T> Entities { get; set; }

        public Repository(MoviesDbContext context) {
            Context = context;
            Entities = Context.Set<T>();
        }

        public int SaveChanges() {
            return Context.SaveChanges();
        }

        public void Dispose() {
            Context.Dispose();
        }
    }
}
