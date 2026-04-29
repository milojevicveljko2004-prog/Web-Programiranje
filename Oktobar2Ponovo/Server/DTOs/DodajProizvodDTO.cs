using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodajProizvodDTO
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        public required int KategorijaID {get; set;} //klijent bira select - lakse je da se option posmatra kao ID

        public double Cena {get; set;}

        public int Kolicina {get; set;}

        public int prodavnicaId {get; set;}
    }
}