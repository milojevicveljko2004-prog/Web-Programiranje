using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RekorderZadatak.Code;

public class PodaciRekord
{
        public required string InformacijaOTakmicenju {get; set;}

        public required DateTime Datum{get; set;}

        public int RekorderID {get; set;}

        public int DisciplinaID {get; set;}
}
