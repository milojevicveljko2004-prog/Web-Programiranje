using System.ComponentModel.DataAnnotations;

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
        public String? BrTelefona {get; set;}

        public List<ProizvodUProdavnici>? ProizvodiUProdavnici {get; set;}
    }
}