using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Rekord
    {
        [Key]
        public int ID {get; set;}

        public required string InformacijaOTakmicenju {get; set;}

        public required DateTime Datum{get; set;}

        public Rekorder? rekorder {get; set;}

        public Disciplina? disciplina {get; set;}
    }
}