using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodajHamburgerDTO
    {
        [MaxLength(100)]
        public required String Naziv {get; set;}

        public bool Prodat {get; set;}

        public int ProdavnicaID {get; set;}
    }
}