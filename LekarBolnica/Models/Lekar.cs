using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Lekar
    {
        [Key]
        public int ID {get; set;}

        public required string Ime {get; set;}

        public required string Prezime {get; set;}

        public required DateTime DatumRodjenja {get; set;}

        public required DateTime DatumDiplomiranja {get; set;}

        public DateTime? DatumDobijanjaLicence {get; set;}

        public List<LekarUBolnici>? LekariUBolnici {get; set;}

        public static implicit operator Lekar(Bolnica v)
        {
            throw new NotImplementedException();
        }
    }
}