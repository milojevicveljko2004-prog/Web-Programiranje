using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class HamburgerContext : DbContext
    {
        public HamburgerContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Prodavnica> Prodavnice {get; set;}

        public DbSet<Hamburger> Hamburgeri {get; set;}

        public DbSet<Sastojak> Sastojci {get; set;}

        public DbSet<SastojakUHamburgeru> SastojciUHamburgeru {get; set;}
    }
}