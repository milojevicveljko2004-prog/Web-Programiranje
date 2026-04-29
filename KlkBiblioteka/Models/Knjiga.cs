using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Knjiga
    {
        [Key]
        public int ID {get; set;}

        public required string Naslov {get; set;}

        public required string ImeAutora {get; set;}

        public int GodinaIzdavanja {get; set;}

        public string? NazivIzdavaca {get; set;}

        public int BrojUEvidencijiBiblioteke {get; set;}

        public Biblioteka? biblioteka {get; set;}

       //Jedna knjiga moze da bude izdavana vise puta
        public List<Izdavanje>? Izdavanja {get; set;}
    }
}