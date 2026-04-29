using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Biblioteka
    {
        [Key]
        public int ID {get; set;}

        public required string Ime {get; set;}

        public string? Adresa {get; set;}

        public string? EmailAdresa {get; set;}

        // public string? PodaciOKnjigamaKojeImaNaPolicama {get; set;}

        // public string? PodaciOIzdavanjuKnjiga {get; set;}

        public List<Knjiga>? Knjige {get; set;}

        public List<Izdavanje>? Izdavanja {get; set;}
    }
}