using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class ProizvodUProdavnici
    {
        [Key]
        public int ID {get; set;}

        public int Kolicina {get; set;}

        public double Cena {get; set;}

        [JsonIgnore]
        public Proizvod? proizvod {get; set;}

        [JsonIgnore]
        public Prodavnica? prodavnica {get; set;}
    }
}