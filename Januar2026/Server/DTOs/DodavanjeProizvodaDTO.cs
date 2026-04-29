using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodavanjeProizvodaDTO
    {
        // [Key]
        // public int ID {get; set;}

        [Required]
        public int ArtikalID {get; set;}

        [Required]
        public int ProdavnicaID {get; set;}

        [Required]
        public double Cena {get; set;}

        [Required]
        public int KolicinaS {get; set;}

        [Required]
        public int KolicinaM {get; set;}

        [Required]
        public int KolicinaL {get; set;}
    }
}