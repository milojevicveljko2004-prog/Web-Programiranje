using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace PrviPrimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredmetController : ControllerBase
    {
        public FakultetContext _context {get; set;}
        public PredmetController(FakultetContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("DodajPredmet")]
        public async Task<ActionResult> DodajPredmet([FromBody] Predmet p)
        {
            if(string.IsNullOrWhiteSpace(p.Naziv) || p.Naziv.Length > 50)
            {
                return BadRequest("Naziv predmeta je prazan ili duzi od 50 karaktera!\n");
            }
            
            if(p.Godina < 0 || p.Godina > 5)
            {
                return BadRequest("Godina mora biti izmedju 0 i 5!\n");
            }

            _context.Predmeti.Add(p);
            await _context.SaveChangesAsync();
            return Ok($"Predmet {p.Naziv} je dodat u bazu. ID = {p.ID} \n");
        }


    }
}

