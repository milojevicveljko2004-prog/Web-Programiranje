using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DodajOcenuDTO
    {
        [Range(1, 10)]
        public required int Vrednost {get; set;}

        public required int FilmID {get; set;}
    }
}