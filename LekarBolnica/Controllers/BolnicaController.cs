using Microsoft.AspNetCore.Mvc;
using Models;
using LekarBolnica.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LekarBolnica.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BolnicaController : ControllerBase
    {
        public BolnicaContext Context {get; set;}

        public BolnicaController(BolnicaContext context)
        {
            Context = context;
        }

        [Route("DodajBolnicu")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeBolnice([FromBody] Bolnica bolnica)
        {
            if(string.IsNullOrWhiteSpace(bolnica.Naziv) || string.IsNullOrWhiteSpace(bolnica.Lokacija)
            || bolnica.BrojOdeljenja<0 || bolnica.BrojOsoblja<0)
            {
                return BadRequest("Neki od argumenata su neispravno prosledjeni! ");
            }

            try
            {
                await Context.Bolnice.AddAsync(bolnica);
                await Context.SaveChangesAsync();

                return Ok($"Bolnica {bolnica.Naziv} je dodata u bazu podataka. ID = {bolnica.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajLekara")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeLekara([FromBody] Lekar lekar)
        {
            if(string.IsNullOrWhiteSpace(lekar.Ime) || string.IsNullOrWhiteSpace(lekar.Prezime))
            {
                return BadRequest("Neki od argumenata su neispravno prosledjeni! ");
            }

            try
            {
                await Context.Lekari.AddAsync(lekar);
                await Context.SaveChangesAsync();

                return Ok($"Lekar {lekar.Ime} {lekar.Prezime} je dodat u bazu podataka. ID = {lekar.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajLekaraUBolnici")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeLekaraUBolnici([FromBody] DodavanjeLekaraUBolniciDto noviLekar)
        {
            if(noviLekar.IDBolnica<=0 || noviLekar.IDLekar<=0)
            {
                return BadRequest("ID ne sme biti manji od 0!");
            }

            try
            {
                var bolnica = await Context.Bolnice.FindAsync(noviLekar.IDBolnica);

                if(bolnica==null)
                {
                    return BadRequest($"Bolnica sa ID-jem {noviLekar.IDBolnica} ne postoji u bazi.");
                }

                var lekar = await Context.Lekari.FindAsync(noviLekar.IDLekar);

                if(lekar==null)
                {
                    return BadRequest($"Lekar sa ID-jem {noviLekar.IDLekar} ne postoji u bazi.");
                }

                var lekarUBolnici = new LekarUBolnici
                {
                    DatumPOtpisivanjaUgovora = noviLekar.DatumPOtpisivanjaUgovora,
                    Specijalnost = noviLekar.Specijalnost,
                    bolnica = bolnica,
                    lekar = lekar
                };

                await Context.LekariUBolnici.AddAsync(lekarUBolnici);
                await Context.SaveChangesAsync();

                return Ok($"Lekar u bolnici je dodat u bazu. ID = {lekarUBolnici.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Pronalazenje informacija o svim lekarima koji rade u odredjenoj bolnici
        [Route("PronadjiInformacijeOLekarima/{IdBolnica}")]
        [HttpGet]
        public async Task<ActionResult> PronadjiLekare([FromRoute] int IdBolnica)
        {
            if(IdBolnica < 0)
            {
                return BadRequest("ID ne sme biti manji od 0!");
            }

            try
            {
                var postoji = await Context.Bolnice.FindAsync(IdBolnica);

                if(postoji == null)
                {
                    return BadRequest($"Bolnica sa ID-jem {IdBolnica} ne postoji!");
                }

                var lekari = await Context.LekariUBolnici
                            .Include(s => s.bolnica)
                            .Include(s => s.lekar)
                            .Where(s => s.bolnica!.ID == IdBolnica)
                            .Select(s => new
                            {
                                Ime = s.lekar!.Ime,
                                Prezime = s.lekar!.Prezime,
                                SpecijalnostKojuObavlja = s.Specijalnost
                            }).ToListAsync();

                if(lekari == null)
                {
                    return BadRequest("Nije pronadjen nijedan lekar.");
                }

                return Ok(lekari);            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Da li bolnica ima zaposlene lekare koji nemaju validnu licencu
        [Route("DaLiImaLekaraBezLicence/{IdBolnica}")]
        [HttpGet]
        public async Task<ActionResult> PronadjiLekareBezLicence([FromRoute] int IdBolnica)
        {
            if(IdBolnica < 0)
            {
                return BadRequest("ID ne sme biti manji od 0!");
            }

            try
            {
               var postoji = await Context.Bolnice.FindAsync(IdBolnica);

               if(postoji==null)
                {
                    return BadRequest("Trazena bolnica ne postoji u bazi.");
                }


                int rezultat = await Context.LekariUBolnici
                            .Include(s => s.lekar)
                            .Include(s => s.bolnica)
                            .Where(s => s.bolnica!.ID == IdBolnica //svi lekari u datoj bolnici
                            && s.lekar!.DatumDobijanjaLicence == null) //nevalidna licenca
                            .CountAsync();

                if(rezultat == 0)
                {
                    return Ok($"Bolnica {postoji.Naziv} nema lekare sa nevalidnom licencom");
                }
                else
                {
                    return Ok($"Bolnica {postoji.Naziv} ima {rezultat} lekara sa nevalidnom licencom");
                }
                            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Ovo na dalje nije na blanketu:

        //Za zadatu bolnicu vrati listu specijalnosti, i za svaku specijalnost listu lekara
        // [Route("VratiSpecijalnostIListuLekara/{IdBolnica}")]
        // [HttpGet]
        // public async Task<ActionResult> VratiSpecijalnostILekare([FromRoute] int IdBolnica)
        // {
        //     if(IdBolnica <= 0)
        //     {
        //         return BadRequest("ID ne sme biti negativan!");
        //     }

        //     try
        //     {
        //         //prvo provera da li trezena bolnica postoji
        //         var bolnica = await Context.Bolnice.FindAsync(IdBolnica);

        //         if(bolnica == null)
        //         {
        //             return BadRequest($"Bolnica sa ID-jem {IdBolnica} ne postoji u bazi.");
        //         }

        //         var specijalnosti = await Context.LekariUBolnici
        //                     .Where(s => s.bolnica!.ID == IdBolnica)
        //                     .GroupBy(s => s.Specijalnost)
        //                     .Select(g => new
        //                     {
        //                         Specijalnost = g.Key,
        //                         Lekari = g.Select(p => new
        //                         {
        //                             Ime = g.

        //                         })
        //                     }).ToListAsync();
        //     }
        //     catch(Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }
    }
}