using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class SendviciContext : DbContext
    {
        public SendviciContext(DbContextOptions options) : base(options)
        {}
        
        public DbSet<Mesto> Mesta {get; set;}

        public DbSet<Prodavnica> Prodavnice {get; set;}

        public DbSet<Sastojak> Sastojci {get; set;}

        public DbSet<SastojakUProdavnici> SastojciUProdavnici {get; set;}

        public DbSet<SastojakUSendvicu> SastojciUSendvicu {get; set;}
    }
}