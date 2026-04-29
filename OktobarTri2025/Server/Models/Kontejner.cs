using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Kontejner
    {
        [Key]
        public int ID {get; set;}

        public required int MaxKapacitet {get; set;}

        public required int TrenutniKapacitet {get; set;}

        public Boja? Boja {get; set;}

        public Fabrika? Fabrika {get; set;}
    }
}