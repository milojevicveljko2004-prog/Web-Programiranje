using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class SastojakUProdavnici
    {
        [Key]
        public int ID {get; set;}

        public required int Kolicina {get; set;}

        public required int Cena {get; set;}

        public Prodavnica? Prodavnica {get; set;}

        public Sastojak? Sastojak {get; set;}

        [JsonIgnore]
        public List<SastojakUSendvicu>? SastojciUSendvicu {get; set;}
    }
}