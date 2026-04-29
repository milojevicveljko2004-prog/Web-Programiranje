using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodajKategorijuDTO
    {
        [MaxLength(100)]
        public required String NazivKategorije {get; set;}

        public required int ProdakcijskaKucaID {get; set;}
    }
}