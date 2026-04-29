using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SastojakUHamburgeru
    {
        [Key]
        public int ID {get; set;}

        public required int Kolicina {get; set;}

        public Sastojak? Sastojak {get; set;}

        public Hamburger? Hamburger {get; set;}
    }
}