using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class StovaristeContext : DbContext
    {
        public StovaristeContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Stovariste> Stovarista {get; set;}

        public DbSet<Materijal> Materijali {get; set;}

        public DbSet<IsporucenMaterijal> IsporuceniMaterijali {get; set;}
    }
}