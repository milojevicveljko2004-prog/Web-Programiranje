using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SastojakUSendvicu
    {
        [Key]
        public int ID { get; set; }

        public int Kolicina { get; set; }

        public Mesto? Mesto { get; set; }

        public SastojakUProdavnici? SastojakUProdavnici { get; set; }
    }
}