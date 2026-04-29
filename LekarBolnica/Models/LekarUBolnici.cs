using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class LekarUBolnici
    {
        [Key]
        public int ID {get; set;}

        public DateTime DatumPOtpisivanjaUgovora {get; set;}

        public string? Specijalnost {get; set;}

        //public int IdentifikacioniBroj { get; set; } //treba da postoji, ali nisam stavio...


        public Bolnica? bolnica {get; set;}

        public Lekar? lekar {get; set;}
    }
}