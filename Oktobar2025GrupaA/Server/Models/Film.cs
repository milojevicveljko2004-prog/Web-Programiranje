using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Film
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        [JsonIgnore]
        public List<Ocena>? Ocene {get; set;}

        public Kategorija? Kategorija {get; set;}
    }
}