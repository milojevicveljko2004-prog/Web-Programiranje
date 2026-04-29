using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Rekorder
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(50)]
        public required string Ime {get; set;}

        [MaxLength(50)]
        public required string Prezime {get; set;}

        public required DateTime DatumRodjenja{get; set;}

        public required char Pol{get; set;}

        [MaxLength(50)]
        public required string Sport {get; set;} //da li treba da bude string?

        public List<Rekord>? rekordi {get; set;}
    }
}