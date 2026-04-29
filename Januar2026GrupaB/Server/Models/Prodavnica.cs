using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Prodavnica
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        [JsonIgnore]
        public List<SastojakUProdavnici>? SastojciUProdavnici {get; set;}

        [JsonIgnore]
        public List<Mesto>? Mesta {get; set;}
    }
}