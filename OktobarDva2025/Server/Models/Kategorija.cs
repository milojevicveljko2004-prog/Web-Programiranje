using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Kategorija //klasa, a ne enum jer zadatak kaze da se kategorije preuzimaju iz baze podataka
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        public List<Proizvod>? Proizvodi {get; set;}
    }
}