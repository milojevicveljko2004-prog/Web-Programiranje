using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Spoj
    {
        [Key]
        public int ID {get; set;}

        [Range(5, 10)]
        public int ocena {get; set;}

        public int StudentID {get; set;}
        public int PredmetID {get; set;}
        public int IspitniRokID {get; set;}

        [JsonIgnore]
        public Student Student {get; set;} = null!;

        public Predmet Predmet {get; set;} = null!;

        public IspitniRok IspitniRok {get; set;} = null!;
    }
}