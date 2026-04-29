using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ProdajProizvodDTO
    {
        [Key]
        public int ID {get; set;}

        public int Kolicina {get; set;}

        public int ProizvodID {get; set;}
        
        public int ProdavnicaID {get; set;}
    }
}