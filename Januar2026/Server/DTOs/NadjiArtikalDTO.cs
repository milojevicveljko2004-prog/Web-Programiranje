using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class NadjiArtikalDTO
    {
        // [Key]
        // public int ID {get; set;}

        public required int ProdavnicaID {get; set;}

        [MaxLength(100)]
        public required String Brend {get; set;}

        public String? Velicina {get; set;}

        public double? CenaOd {get; set;}

        public double? CenaDo {get; set;}
    }
}