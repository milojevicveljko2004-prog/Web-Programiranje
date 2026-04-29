using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Materijal
    {
        [Key]
        public int ID {get; set;}

        public required string Sifra {get; set;}

        public required string Naziv {get; set;}

        public required float Cena {get; set;}

        public required string NazivProizvodjaca {get; set;}

        public List<IsporucenMaterijal>? IsporuceniMaterijali {get; set;}
    }
}