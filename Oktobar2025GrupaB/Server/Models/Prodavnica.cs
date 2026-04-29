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

        public required int Kapacitet {get; set;} //koliko hamburgera mogu istovremeno biti u pripremi

        [JsonIgnore]
        public List<Hamburger>? Hamburgeri {get; set;}
    }
}