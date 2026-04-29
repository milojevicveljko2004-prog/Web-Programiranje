using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodajFilmDTO
    {
        [MaxLength(100)]
        public required String NazivFilma {get; set;}

        public required int KategorijaID {get; set;}
    }
}