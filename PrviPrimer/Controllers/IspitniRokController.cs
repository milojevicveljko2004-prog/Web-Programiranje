using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Models;

namespace PrviPrimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IspitniRokController : ControllerBase
    {
        public FakultetContext _context {get; set;}

        public IspitniRokController(FakultetContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("DodajIspitniRok")]
        public async Task<ActionResult> DodajIspitniRok([FromBody] IspitniRok isp)
        {
            if(string.IsNullOrWhiteSpace(isp.Naziv))
            {
                return BadRequest("Naziv ispitnog roka je prazan! \n");
            }

            if(isp.Naziv.Length > 50)
            {
                return BadRequest("Naziv ispitnog roka ne sme biti duzi od 50 karaktera! \n");
            }

            _context.Ispiti.Add(isp);
            await _context.SaveChangesAsync();
            return Ok("Ispitni rok je dodat u bazu. \n");
        }
    }
}