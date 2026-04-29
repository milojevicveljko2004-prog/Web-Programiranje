using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Bolnica
    {
        [Key]
        public int ID {get; set;}

        public required string Naziv {get; set;}

        public required string Lokacija {get; set;}

        public required int BrojOdeljenja {get; set;}

        public required int BrojOsoblja {get; set;}

        public string? BrojTelefona {get; set;}

        public List<LekarUBolnici>? LekariUBolnici {get; set;}
    }
}