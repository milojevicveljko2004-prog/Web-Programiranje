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

        public String? Lokacija {get; set;}

        [Phone]
        public String? BrojTelefona {get; set;}

        [JsonIgnore]
        public List<ProizvodUProdavnici>? proizvodiUProdavnici {get; set;}
    }
}