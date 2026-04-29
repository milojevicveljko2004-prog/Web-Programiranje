using Models;

namespace OktobarTri2025.DTOs
{
    public class ProizvodUProdavniciDTO
    {
        public int ProizvodID {get; set;}

        public required String NazivProizvoda {get; set;}

        public required Kategorija kategorija {get; set;}
    }
}