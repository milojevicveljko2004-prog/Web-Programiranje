using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class IsporucenMaterijal
    {
        [Key]
        public int ID {get; set;}

        public required DateTime DatumIsporuke {get; set;}

        public required int KolicinaMaterijala {get; set;}

        public Materijal? materijal {get; set;}

        public Stovariste? stovariste {get; set;}
    }
}