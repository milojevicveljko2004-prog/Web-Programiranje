namespace OktobarTri2025.DTOs
{
    public class ProdavnicaDTO
    {
        public int ID { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string? Lokacija { get; set; }
        public string? BrTelefona { get; set; }

        public List<ProizvodZaPrikazDTO> Proizvodi { get; set; } = new();
    }
}