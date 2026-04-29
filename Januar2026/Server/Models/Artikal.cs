using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Artikal
    {
        [Key]
        public int ID {get; set;}

        [MaxLength(100)]
        public required String Brend {get; set;}

        //Treba i ovo, ali nisam stavio... Da ne bi sve menjao sad, koristicu i dalje sifru umesto naziva
        // [MaxLength(100)] 
        // public required String Naziv {get; set;}

        [MaxLength(20)]
        public required String SifraModela {get; set;}

        //ovo cu da ostavim za kraj, jer je vrv komplikovano
        public String? Slika {get; set;}

        [JsonIgnore]
        public List<ArtikalUProdaji>? ArtikliUProdaji {get; set;}
    }
}