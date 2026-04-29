namespace OktobarTri2025.DTOs
{
    public class ProizvodZaPrikazDTO
    {
        public int IDProizvodUProdavnici { get; set; }
        public int IDProizvoda { get; set; }
        public string NazivProizvoda { get; set; } = string.Empty;
        public string Kategorija { get; set; } = string.Empty;
        public double Cena { get; set; }
        public int Kolicina { get; set; }
    }
}