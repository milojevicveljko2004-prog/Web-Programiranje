namespace Models
{
    public class Izdavanje
    {
        public int ID {get; set;}

        public DateTime DatumIzdavanja {get; set;}

        public DateTime DatumVracanja {get; set;}

//Jedno izdavanje se desava za jednu knjigu i u jednoj biblioteci.
//Npr izdajem knjigu Harry Potter. To je jedna knjiga i fizicki se nalazi u jednoj biblioteci.
        public Biblioteka? biblioteka {get; set;}

        public Knjiga? knjiga {get; set;}
    }
}