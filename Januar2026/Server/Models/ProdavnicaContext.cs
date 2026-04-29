using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class ProdavnicaContext : DbContext
    {
        public ProdavnicaContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Artikal> Artikli {get; set;}

        public DbSet<Prodavnica> Prodavnice {get; set;}

        public DbSet<ArtikalUProdaji> ArtikliUProdaji {get; set;}
    }
}