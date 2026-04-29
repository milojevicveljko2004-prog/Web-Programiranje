using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Hamburger
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        public bool Prodat {get; set;}

        public Prodavnica? Prodavnica {get; set;}

        [JsonIgnore]
        public List<SastojakUHamburgeru>? SastojciUHamburgeru {get; set;}
    }
}