using Microsoft.AspNetCore.Mvc;
using Models;

namespace PrviPrimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpojController : ControllerBase
    {
        public FakultetContext _context{get; set;}

        public SpojController(FakultetContext context)
        {
            _context = context;
        }

        // [HttpPost]
        // [Route("DodajVezu")]
        // public async Task<ActionResult> DodajVezuFakultet([FromBody] Spoj fakultet)
        // {
        //     if(fakultet.StudentID<0)
        //     {
        //         return BadRequest("Student ID ne moze biti manji od 0.");
        //     }
        //     if(fakultet.PredmetID<0)
        //     {
        //         return BadRequest("Predmet ID ne moze biti manji od 0.");
        //     }
        //     if(fakultet.IspitniRokID<0)
        //     {
        //         return BadRequest("Ispitni rok ID ne moze biti manji od 0.");
        //     }

        //     if(fakultet.ocena < 5 || fakultet.ocena > 10)
        //     {
        //         return BadRequest("Ocena mora biti izmedju 5 i 10!");
        //     }

        //     _context.Spojevi.Add(fakultet);
        //     await _context.SaveChangesAsync();
        //     return Ok("Spoj veze je dodat u bazu. ");
        // }
    }
}