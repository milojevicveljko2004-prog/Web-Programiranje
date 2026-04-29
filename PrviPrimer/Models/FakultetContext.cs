using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class FakultetContext : DbContext
    {
        public FakultetContext(DbContextOptions options) : base(options)
        {}
        public DbSet<Student> Studenti {get; set;}
        public DbSet<Predmet> Predmeti {get; set;}
        public DbSet<IspitniRok> Ispiti{get; set;}

        // [JsonIgnore]
        // public DbSet<Spoj> Spojevi{get; set;}

        public int Ocena {get; set;}

        //funkcija za mapiranje klasa tj tabela
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>() //Student ima vise veza spojeva(listu), a spoj ima jednog studenta
                        .HasMany(s => s.StudentiPredmeti)
                        .WithOne(sp => sp.Student);

            modelBuilder.Entity<Predmet>()
                        .HasMany(p => p.PredmetiStudenti)
                        .WithOne(sp => sp.Predmet);

            modelBuilder.Entity<IspitniRok>()
                        .HasMany(i => i.IspitniRokovi)
                        .WithOne(sp => sp.IspitniRok);         
        }
    }
}