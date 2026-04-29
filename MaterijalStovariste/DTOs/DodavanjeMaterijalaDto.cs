namespace MaterijalStovariste.DTOs
{
    public class DodavanjeMaterijalaDto
    {
        public int StovaristeID {get; set;}

        public int MaterijalID {get; set;}

        public required DateTime DatumIsporuke {get; set;}

        public required int KolicinaMaterijala {get; set;}
    }
}