using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class ProizvodUProdavnici //klasa veze
    {
        [Key]
        public int ID {get; set;}

        public double Cena {get; set;}

        public int Kolicina {get; set;}

        public Proizvod? proizvod {get; set;}

        public Prodavnica? prodavnica {get; set;}
    }
}