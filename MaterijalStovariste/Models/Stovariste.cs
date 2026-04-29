using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Stovariste
    {
        [Key]
        public int ID {get; set;}

        public required string Ime {get; set;}

        public required string Adresa {get; set;}

        public required string BrojTelefona {get; set;}

        public List<IsporucenMaterijal>? IsporuceniMaterijali {get; set;}
    }
}