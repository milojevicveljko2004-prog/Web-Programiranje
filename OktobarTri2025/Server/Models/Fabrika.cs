using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Fabrika
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        //smatram da je ovo max broj kontejnera koji fabrika moze da poseduje
        //ako hocu da bude trenutni broj kontejnera, onda bi trebalo svaki put kada se kontejner doda u fabriku da se ovoj podatak inkrementira
        //ali to mozda i nema smisla, jer trenutni broj kontejnera svakako moze da se sazna iz liste preko Kontejneri.size() ili tako nesto?
        public required int BrKontejnera {get; set;}

        [JsonIgnore]
        public List<Kontejner>? Kontejneri {get; set;}
    }
}