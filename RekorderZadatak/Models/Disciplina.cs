using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Disciplina
    {
        [Key]
        public int ID {get; set;}

        [Required]
        public required string Naziv {get; set;}

        public required DateTime DatumOd {get; set;}

        public int TrenutniBrTakmicara {get; set;}

        public string? Opis {get; set;}

        public List<Rekord>? rekordi {get; set;}
    }
}