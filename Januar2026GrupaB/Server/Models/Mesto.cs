using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Mesto
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        public int UkupnaCena {get; set;}

        public Prodavnica? Prodavnica {get; set;}

        //ali mesto ima svoju listu sastojaka u prodavnici, znaci nezavisnu od one koja je u prodavnici
        //jedno mesto=jedan hamburger. U svakom hamburgeru se nalaze sastojci.
        [JsonIgnore]
        public List<SastojakUSendvicu>? SastojciUSendvicu {get; set;}
    }
}