using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class ProdavnicaContext : DbContext
    {
        public ProdavnicaContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Prodavnica> Prodavnice {get; set;}

        public DbSet<Proizvod> Proizvodi {get; set;}

        public DbSet<ProizvodUProdavnici> ProizvodiUProdaji {get; set;}

        public DbSet<Kategorija> Kategorije {get; set;}
    }
}