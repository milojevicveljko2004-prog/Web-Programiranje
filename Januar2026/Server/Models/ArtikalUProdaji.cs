using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class ArtikalUProdaji
    {
        [Key]
        public int ID {get; set;}

        [Required]
        public double Cena {get; set;}

        [Required]
        public int KolicinaS {get; set;}

        [Required]
        public int KolicinaM {get; set;}

        [Required]
        public int KolicinaL {get; set;}

        public Artikal? Artikal {get; set;}

        //[JsonIgnore]
        public Prodavnica? Prodavnica {get; set;}
    }
}