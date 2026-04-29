using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Sastojak
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        public required int Debljina {get; set;}

        [JsonIgnore]
        public List<SastojakUHamburgeru>? SastojciUHamburgeru {get; set;}
    }
}