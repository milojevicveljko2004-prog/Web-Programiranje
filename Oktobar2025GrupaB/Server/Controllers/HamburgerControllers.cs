using System.Runtime.InteropServices;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HamburgerController : ControllerBase
    {
        public HamburgerContext Context {get; set;}

        public HamburgerController(HamburgerContext context)
        {
            this.Context = context;
        }

        [Route("DodajProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
        {
            if(string.IsNullOrWhiteSpace(prodavnica.Naziv) || prodavnica.Kapacitet<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Prodavnice.AddAsync(prodavnica);
                await Context.SaveChangesAsync();

                return Ok($"Prodavnica {prodavnica.Naziv} je dodata u bazu. ID = {prodavnica.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajHamburger")]
        [HttpPost]
        public async Task<ActionResult> DodajHamburger([FromBody] DodajHamburgerDTO dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Naziv) || dto.ProdavnicaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var prodavnica = await Context.Prodavnice.FindAsync(dto.ProdavnicaID);
                if(prodavnica == null)
                {
                    return BadRequest($"Prodavnica sa ID={dto.ProdavnicaID} ne postoji u bazi!");
                }

                //popunjen je kapacitet prodavnice
                if(prodavnica.Hamburgeri != null && prodavnica.Hamburgeri.Count() == prodavnica.Kapacitet - 1)
                {
                    return BadRequest("Kapacitet prodavnice je popunjen!");
                }

                var hamburger = new Hamburger
                {
                    Naziv = dto.Naziv,
                    Prodat = dto.Prodat,
                    Prodavnica = prodavnica
                };

                await Context.Hamburgeri.AddAsync(hamburger);
                await Context.SaveChangesAsync();

                return Ok($"Hamburger {hamburger.Naziv} je dodat u bazu. ID={hamburger.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajSastojak")]
        [HttpPost]
        public async Task<ActionResult> DodajSastojak([FromBody] Sastojak sastojak)
        {
            if(string.IsNullOrWhiteSpace(sastojak.Naziv) || sastojak.Debljina<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Sastojci.AddAsync(sastojak);
                await Context.SaveChangesAsync();

                return Ok($"Sastojak {sastojak.Naziv} je dodat u bazu. ID={sastojak.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Dugme Dodaj na klijentskoj strani: Dodaje sastojak u hamburger.
        [Route("DodajSastojakUHamburger")]
        [HttpPost]
        public async Task<ActionResult> DodajSastojakUHamburger([FromBody] SastojakUHamburgerDTO dto)
        {
            if(dto.HamburgerID<=0 || dto.SastojakID<=0 || dto.Kolicina<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var hamburger = await Context.Hamburgeri.FindAsync(dto.HamburgerID);
                if(hamburger==null)
                {
                    return BadRequest($"Hamburger ID={dto.HamburgerID} ne postoji u bazi.");
                }

                var sastojak = await Context.Sastojci.FindAsync(dto.SastojakID);
                if(sastojak==null)
                {
                    return BadRequest($"Sastojak ID={dto.SastojakID} ne postoji u bazi.");
                }

                var sastojakZaHamburger = new SastojakUHamburgeru
                {
                    Kolicina = dto.Kolicina,
                    Sastojak = sastojak,
                    Hamburger = hamburger
                };

                await Context.SastojciUHamburgeru.AddAsync(sastojakZaHamburger);
                await Context.SaveChangesAsync();

                return Ok($"Sastojak {sastojak.Naziv} je dodat u hamburger {hamburger.Naziv}. ID = {sastojakZaHamburger.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PribaviProdavnice")]
        [HttpGet]
        public async Task<ActionResult> PribaviProdavnice()
        {
            try
            {
                var result = await Context.Prodavnice
                            .Include(s => s.Hamburgeri!)
                            .ThenInclude(p => p.SastojciUHamburgeru!)
                            .ThenInclude(r => r.Sastojak)
                            .ToListAsync();

                return Ok(result.Select(s =>new
                {
                    ID = s.ID,
                    Naziv = s.Naziv,
                    Kapacitet = s.Kapacitet,
                    Hamburgeri = s.Hamburgeri!.Select(p => new
                    {
                        ID = p.ID,
                        Naziv = p.Naziv,
                        Prodat = p.Prodat,
                        SastojciUHamburgeru = p.SastojciUHamburgeru!.Select(r => new
                        {
                            ID = r.ID,
                            Kolicina = r.Kolicina,
                            NazivSastojka = r.Sastojak!.Naziv,
                            Debljina = r.Sastojak!.Debljina
                        }).ToList()
                    }).ToList()
                }));          
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiSveSastojke")]
        [HttpGet]
        public async Task<ActionResult> VratiSveSastojke()
        {
            try
            {
                var result = await Context.Sastojci.ToListAsync();

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Dugme Kupi na klijentskoj strani
        //Kada se kupi hamburger status Prodat se postavi na true i resetuju se prilozi - vise nema priloge
        [Route("KupiHamburger/{hamburgerID}")]
        [HttpPut]
        public async Task<ActionResult> KupiHamburger(int hamburgerID)
        {
            if(hamburgerID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var hamburger = await Context.Hamburgeri
                                .Include(s => s.SastojciUHamburgeru)
                                .FirstOrDefaultAsync(s => s.ID == hamburgerID);
                if(hamburger==null)
                {
                    return BadRequest($"Nije nadjen hamburger sa ID={hamburgerID}.");
                }

                hamburger.Prodat = true;
                //brise sastojke iz hamburgera
                Context.SastojciUHamburgeru.RemoveRange(hamburger.SastojciUHamburgeru!);

                await Context.SaveChangesAsync();
                return Ok($"Hamburger {hamburger.Naziv} je kupljen.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}