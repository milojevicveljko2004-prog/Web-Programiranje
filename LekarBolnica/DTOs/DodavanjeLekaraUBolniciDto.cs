namespace LekarBolnica.DTOs;

    public class DodavanjeLekaraUBolniciDto
    {
        public required int IDBolnica {get; set;}

        public required int IDLekar {get; set;}

        public DateTime DatumPOtpisivanjaUgovora {get; set;}

        public string? Specijalnost {get; set;}
    }
