using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class FabrikaContext : DbContext
    {
        public FabrikaContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Fabrika> Fabrike {get; set;}

        public DbSet<Kontejner> Kontejneri {get; set;}

        public DbSet<Boja> Boje {get; set;}
    }
}