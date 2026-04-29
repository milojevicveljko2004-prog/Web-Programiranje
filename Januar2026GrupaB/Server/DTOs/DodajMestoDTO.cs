using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodajMestoDTO
    {
        [MaxLength(100)]
        public required String Naziv {get; set;}

        public required int ProdavnicaID {get; set;}
    }
}