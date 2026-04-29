using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class TakmicenjeContext : DbContext
    {
        //konstruktor
        public TakmicenjeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Rekorder> Rekorderi {get; set;}

        public DbSet<Disciplina> Discipline {get; set;}

        public DbSet<Rekord> Rekordi {get; set;}
    }
}