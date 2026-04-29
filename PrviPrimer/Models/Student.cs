using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Student
    {
        [Key]
        public int ID {get; set;}

        [Required] //onemogucava da se u bazi upise vrednost NULL. Isto kao da smo stavili NOT NULL.
        [Range(1, 99999)]
        public int indeks{get; set;}
   
        [Required]
        [StringLength(50)]
        public required string Ime {get; set;}

        [Required]
        [StringLength(50)] 
        public required string Prezime{get; set;}

        public List<Spoj> StudentiPredmeti {get; set;} = null!;
    }
}