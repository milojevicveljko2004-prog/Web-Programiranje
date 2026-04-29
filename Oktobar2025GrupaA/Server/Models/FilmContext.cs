using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class FilmContext : DbContext
    {
        public FilmContext(DbContextOptions options) : base(options)
        {}

        public DbSet<ProdukcijskaKuca> ProdukcijskeKuce {get; set;}

        public DbSet<Film> Filmovi {get; set;}

        public DbSet<Kategorija> Kategorije {get; set;}

        public DbSet<Ocena> Ocene {get; set;}
    }
}