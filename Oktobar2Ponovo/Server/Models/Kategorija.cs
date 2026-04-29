using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Kategorija
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        [JsonIgnore]
        public List<Proizvod>? proizvodi {get; set;}
    }
}