using DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FabrikaController : ControllerBase
    {
        public FabrikaContext Context {get; set;}

        public FabrikaController(FabrikaContext context)
        {
            this.Context = context;
        }

        [Route("DodajFabriku")]
        [HttpPost]
        public async Task<ActionResult> DodajFabriku([FromBody] Fabrika fabrika)
        {
            if(fabrika==null || string.IsNullOrWhiteSpace(fabrika.Naziv) || fabrika.BrKontejnera<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Fabrike.AddAsync(fabrika);
                await Context.SaveChangesAsync();

                return Ok($"Fabrika {fabrika.Naziv} ID={fabrika.ID} je dodata u bazu.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //inicijalno su svi kontejneri prazni sto znaci da atribut TrenutniKapacitet treba da bude 0. Boja ce biti null.
        //Znaci ne unosi korisnik sve - pa mi treba DTO klasa
        [Route("DodajKontejner")]
        [HttpPost]
        public async Task<ActionResult> DodajKontejner([FromBody] DodajKontejnerDTO dto)
        {
            if(dto==null || dto.FabrikaID<=0 || dto.MaxKapacitet<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var fabrika = await Context.Fabrike.FindAsync(dto.FabrikaID);
                if(fabrika==null)
                {
                    return BadRequest($"Fabrika sa ID={dto.FabrikaID} ne postoji u bazi!");
                }

                //provera da li se dodavanjem kontejnera prevazilazi maksimalan broj kontejnera u fabrici
                var brojKontejnera = await Context.Kontejneri
                                .Where(s => s.Fabrika!.ID == dto.FabrikaID)
                                .CountAsync();

                if(brojKontejnera>=fabrika.BrKontejnera)
                {
                    return BadRequest("Max broj kontejnera u fabrici je prevazidjen.");
                }                

                var kontejner = new Kontejner
                {
                    MaxKapacitet = dto.MaxKapacitet,
                    TrenutniKapacitet = 0,
                    Boja = null,
                    Fabrika = fabrika
                };

                await Context.Kontejneri.AddAsync(kontejner);
                await Context.SaveChangesAsync();

                return Ok($"Kontejner max kapaciteta {kontejner.MaxKapacitet} je dodat u bazu. ID = {kontejner.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajBoju")]
        [HttpPost]
        public async Task<ActionResult> DodajBoju([FromBody] Boja boja)
        {
            if(boja==null || string.IsNullOrWhiteSpace(boja.Naziv))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Boje.AddAsync(boja);
                await Context.SaveChangesAsync();

                return Ok($"Boja {boja.Naziv} je dodata u bazu. ID={boja.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PribaviBoje")]
        [HttpGet]
        public async Task<ActionResult> PribaviBoje()
        {
            try
            {
                var boje = await Context.Boje.ToListAsync();

                return Ok(boje);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajBojuUKontejner")]
        [HttpPut]
        public async Task<ActionResult> DodajBojuUKontejner([FromBody] DodajBojuUKontejnerDTO dto)
        {
            if(dto==null || dto.BojaID<=0 || dto.FabrikaID<=0) //kolicinu cu da omogucim da bude negativna da bi mogo da smanjujem kolicinu
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                //trazi kontejner u kome je moguce dodati odabranu boju u unetoj kolicini
                //znaci prvo se proverava da li vec postoji kontejner sa zadatom bojom i da li u njemu ima jos mesta
                //ako ne postoji takav kontejner, boju sipati u prazan kontejner
                //ako nema dovoljno praznih kontejnera ispisati poruku greske

                var fabrika = await Context.Fabrike.FindAsync(dto.FabrikaID);
                if(fabrika==null)
                {
                    return BadRequest($"Fabrika da ID={dto.FabrikaID} ne postoji u bazi!");
                }

                var boja = await Context.Boje.FindAsync(dto.BojaID);
                if(boja==null)
                {
                    return BadRequest($"Boja sa ID={dto.BojaID} ne postoji u bazi.");
                }

                var kontejnerZaSipanje = await Context.Kontejneri      
                                    .Where(s => s.Boja!.ID == dto.BojaID)
                                    .Where(s => s.TrenutniKapacitet+dto.Kolicina <= s.MaxKapacitet)
                                    .Where(s => s.Fabrika!.ID == dto.FabrikaID)
                                    .FirstOrDefaultAsync();

                if(kontejnerZaSipanje == null)
                {
                    //treba uzeti kontejner bez boje
                    kontejnerZaSipanje = await Context.Kontejneri
                                    .Where(s => s.Boja == null)
                                    .Where(s => s.TrenutniKapacitet == 0)
                                    .Where(s => s.Fabrika!.ID == dto.FabrikaID)
                                    .Where(s => dto.Kolicina<=s.MaxKapacitet)
                                    .FirstOrDefaultAsync();

                    if(kontejnerZaSipanje == null)
                    {
                        return BadRequest("Nije nadjen cak ni prazan kontejner!");
                    }       

                    kontejnerZaSipanje.Boja = boja;         
                }

                kontejnerZaSipanje.TrenutniKapacitet += dto.Kolicina;
                await Context.SaveChangesAsync();

                return Ok($"Kontejner {kontejnerZaSipanje.ID} je dosipan na {kontejnerZaSipanje.TrenutniKapacitet}. Boja: {kontejnerZaSipanje.Boja!.Naziv}");                   
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ObrisiKontejner")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiKontejner(int id)
        {
            try
            {
                var k = await Context.Kontejneri.FindAsync(id);
                if(k==null)
                {
                    return BadRequest("Ne postoji trazeni kontejner u bazi");
                }

                Context.Kontejneri.Remove(k);
                await Context.SaveChangesAsync();

                return Ok("Brisanje uspesno!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiSveFabrike")] //ali treba da ih vrati sa kontejnerima
        [HttpGet]
        public async Task<ActionResult> VratiSveFabrike()
        {
            try
            {
                var result = await Context.Fabrike
                            .Include(s => s.Kontejneri!)
                            .ThenInclude(p => p.Boja)
                            .ToListAsync();

                return Ok(result.Select(s => new
                {
                    id = s.ID,
                    naziv = s.Naziv,
                    brKontejnera = s.BrKontejnera,
                    kontejneri = s.Kontejneri!.Select(p => new
                    {
                        id = p.ID,
                        maxKapacitet = p.MaxKapacitet,
                        trenutniKapacitet = p.TrenutniKapacitet,
                        boja = p.Boja
                    })
                }));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiSveKontejnere")]
        [HttpGet]
        public async Task<ActionResult> VratiSveKontejnere()
        {
            try
            {
                var result = await Context.Kontejneri
                            .Include(s => s.Boja)
                            .Include(s => s.Fabrika)
                            .ToListAsync();

                return Ok(result);            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}   