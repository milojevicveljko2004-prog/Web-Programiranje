using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Proizvod
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        public Kategorija? kategorija {get; set;}

        [JsonIgnore]
        public List<ProizvodUProdavnici>? proizvodiUProdavnici {get; set;}
    }
}