using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class BolnicaContext : DbContext
    {
        public BolnicaContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Bolnica> Bolnice {get; set;}

        public DbSet<Lekar> Lekari {get; set;}

        public DbSet<LekarUBolnici> LekariUBolnici {get; set;}
    }
}