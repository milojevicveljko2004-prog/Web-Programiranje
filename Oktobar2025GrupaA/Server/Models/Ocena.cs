using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    public class Ocena
    {
        [Key]
        public int ID {get; set;}

        [Range(1, 10)]
        public required int Vrednost {get; set;}

        public Film? Film {get; set;}
    }
}