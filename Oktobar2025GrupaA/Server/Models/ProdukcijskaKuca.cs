using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models //Model: ProdukcijskaKuca -> Kategorija -> Film -> Ocena
{
    public class ProdukcijskaKuca
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Naziv {get; set;}

        [JsonIgnore]
        public List<Kategorija>? Kategorije {get; set;}
        //Objasnjenje: Ako stavimo kategorija=Horror to NE znaci da ce ta prod. kuca da ima sve horror filmove!
        //Jer kategorija ima i pokazivac na produkcijsku kucu. Pa mozemo imati vise kategorija sa nazivom Horror
        //Kategorija ID=1, Naziv="Horror", ProdukcijskaKucaID=1 i ona ima listu svojih filmova
        //Kategorija ID=2, Naziv="Horror", ProdukcijskaKucaID=2 ona ima neku drugu listu filmova
    }
}