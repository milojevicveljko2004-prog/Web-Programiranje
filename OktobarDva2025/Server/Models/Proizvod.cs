using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Proizvod
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(200)]
        public required String Naziv {get; set;}

        public required Kategorija kategorija {get; set;}

        //public int IDKategorije {get; set;}

        public List<ProizvodUProdavnici>? ProizvodiUProdavnici {get; set;}
    }
}