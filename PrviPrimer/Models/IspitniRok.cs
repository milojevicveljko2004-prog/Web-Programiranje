using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    [Display(Name = "Ispitni rok")]
    public class IspitniRok
    {
        [Key]
        public int ID {get; set;}

        [Required]
        [MaxLength(50)]
        public required string Naziv {get; set;}

        //[JsonIgnore]
        public List<Spoj> IspitniRokovi {get; set;} = null!;
    }
}